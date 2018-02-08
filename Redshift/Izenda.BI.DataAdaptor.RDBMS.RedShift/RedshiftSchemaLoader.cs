using Izenda.BI.DataAdaptor.SQL.SchemaLoader;
using System;
using System.Collections.Generic;
using Izenda.BI.Framework.Models;
using Izenda.BI.Logging;
using Npgsql;
using Dapper;
using System.Data;
using System.Linq;
using Izenda.BI.RDBMS.Constants;
using Izenda.BI.Resource;
using Izenda.BI.Framework.Exceptions;
using Izenda.BI.DataAdaptor.RDBMS.Redshift.Constants;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift
{
    public class RedshiftSchemaLoader : ISchemaLoader
    {
        private const string EXCLUDE_SCHEMAS = @"'pg_toast', 'pg_internal','pg_temp_1','pg_catalog','information_schema'";

        private const string SQL_TYPE_TABLE = "BASE TABLE";
        private const string SQL_TYPE_VIEW = "VIEW";
        private const string SQL_TYPE_FUNCTION = "FUNCTION";

        private const string LOAD_SCHEMA_FUNCTION = @"
                            SELECT r.routine_schema, r.routine_name, r.routine_type
                            FROM pg_proc pp
                            INNER JOIN pg_namespace pn ON (pp.pronamespace = pn.oid)
                            INNER JOIN pg_language pl ON (pp.prolang = pl.oid)
                            INNER JOIN information_schema.routines r ON r.routine_name = pp.proname AND r.routine_schema = pn.nspname
                            WHERE r.routine_schema = @schema AND r.routine_type = @type AND pp.proretset = '{0}'";

        public List<QuerySourceParameter> GetQuerySourceParameters(string connectionString)
        {
            return this.GetQuerySourceParameters(connectionString, string.Empty, string.Empty);
        }

        public List<QuerySourceParameter> GetQuerySourceParameters(string connectionString, string specificSchema, string specificName)
        {
            var dataTypeAdaptor = new RedshiftSupportDataType();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                var sql = @"SELECT specific_schema
	                                ,specific_name
	                                ,parameter_mode
	                                ,is_result
	                                ,parameter_name
	                                ,data_type
	                                ,ordinal_position
                            FROM information_schema.parameters";

                if (!string.IsNullOrEmpty(specificSchema) && !string.IsNullOrEmpty(specificName))
                {
                    sql += " WHERE specific_schema = @SPECIFIC_SCHEMA AND specific_name=@SPECIFIC_NAME ";
                }

                var parametes =
                    conn.Query<dynamic>(sql, new { SPECIFIC_SCHEMA = specificSchema, SPECIFIC_NAME = specificName })
                        .Select(s => new QuerySourceParameter
                        {
                            Name = s.parameter_name,
                            QuerySourceName = s.specific_name,
                            Category = s.specific_schema,
                            DataType = s.data_type,
                            IzendaDataType = dataTypeAdaptor.GetIzendaDataType(s.data_type),
                            InputMode = s.parameter_mode.Equals("IN", StringComparison.OrdinalIgnoreCase),
                            Result = s.is_result.Equals("YES", StringComparison.OrdinalIgnoreCase),
                            Position = s.ordinal_position,
                            Value = DBNull.Value,
                            AllowDistinct = dataTypeAdaptor.GetAllowDistinct(s.data_type)
                        }).ToList();

                return parametes;
            }
        }

        public List<QuerySourceField> LoadCustomQuerySourceFields(string connectionString, string customQueryDefinition)
        {
            var result = new List<QuerySourceField>();
            var dataTypeAdaptor = new RedshiftSupportDataType();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                NpgsqlCommand command = new NpgsqlCommand(customQueryDefinition, conn);
                command.CommandType = CommandType.Text;
                NpgsqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly);
                DataTable schema = reader.GetSchemaTable();

                for (int i = 0; i < schema.Rows.Count; i++)
                {
                    string dataType = reader.GetDataTypeName(i);

                    result.Add(
                        new QuerySourceField
                        {
                            Name = schema.Rows[i]["ColumnName"].ToString() ?? "",
                            DataType = dataType,
                            IzendaDataType = dataTypeAdaptor.GetIzendaDataType(dataType),
                            AllowDistinct = dataTypeAdaptor.GetAllowDistinct(dataType),
                            ExtendedProperties = "",
                            Position = Convert.ToInt32(schema.Rows[0]["ColumnOrdinal"].ToString())
                        }
                    );
                }
            }

            return result;
        }

        public List<QuerySourceField> LoadFields(string connectionString, string type, string categoryName, string querySourceName, bool rollbackSP, List<QuerySourceParameter> parameters = null, bool ignoreError = true, int commandTimeout = 500, ILog log = null)
        {
            var result = new List<QuerySourceField>();
            if (type.Equals(SQLQuerySourceType.Table, StringComparison.OrdinalIgnoreCase) || type.Equals(SQLQuerySourceType.View, StringComparison.OrdinalIgnoreCase))
            {
                result = LoadFieldsFromTable(connectionString, categoryName, querySourceName);
            }
            else if (type.Equals(SQLQuerySourceType.Procedure, StringComparison.OrdinalIgnoreCase))
            {
                //Redhisft does not support Store Procedure, no need to handle here
            }

            return result;
        }

        private List<QuerySourceField> LoadFieldsFromTable(string connectionString, string schemaName = "", string tableName = "")
        {
            var result = new List<QuerySourceField>();
            var dataTypeAdaptor = new RedshiftSupportDataType();

            string sql = @"SELECT DISTINCT c.column_name, c.data_type, c.ordinal_position, c.table_schema, c.table_name
                           FROM information_schema.Columns c";

            if (!string.IsNullOrWhiteSpace(tableName) && !string.IsNullOrWhiteSpace(schemaName))
            {
                sql += @" WHERE c.table_schema = @TableSchema AND c.table_name = @TableName ";
            }

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                result = conn.Query<dynamic>(sql, new { TableSchema = schemaName, TableName = tableName })
                             .Select(s => new QuerySourceField
                             {
                                 Name = s.column_name,
                                 CategoryName = s.table_schema ?? "",
                                 QuerySourceName = s.table_name ?? "",
                                 DataType = s.data_type,
                                 IzendaDataType = dataTypeAdaptor.GetIzendaDataType(s.data_type),
                                 AllowDistinct = dataTypeAdaptor.GetAllowDistinct(s.data_type),
                                 Position = s.ordinal_position,
                                 ExtendedProperties = ""
                             })
                                    .Where(x => !string.IsNullOrEmpty(x.IzendaDataType))
                                    .ToList();
            }

            return result;
        }

        public List<QuerySourceField> LoadQuerySourceFields(string connectionString)
        {
            return LoadFieldsFromTable(connectionString);
        }

        public List<Relationship> LoadRelationships(string connectionString, List<string> schemas = null)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                string sql = @"SELECT kcu.constraint_name as fk_name,
                                    kcu.constraint_schema as jionschema,
                                    kcu.table_name as jiontable,
                                    kcu.column_name as jioncolumn,
                                    ccu.constraint_schema as foreignschema,
                                    ccu.table_name as foreigntable,
                                    ccu.column_name as foreigncolumn
                            FROM information_schema.key_column_usage as kcu
                            INNER JOIN information_schema.constraint_column_usage as ccu on ccu.constraint_name = kcu.constraint_name
                            INNER JOIN information_schema.referential_constraints as rc on rc.constraint_name = kcu.constraint_name";

                if (schemas != null && schemas.Count() > 0)
                {
                    sql += $" WHERE kcu.constraint_schema IN ({schemas.Aggregate((s1, s2) => $"'{s1}', '{s2}'")})";
                }

                var relationships = conn.Query<dynamic>(sql)
                                              .Select(r => new Relationship
                                              {
                                                  JoinQuerySourceName = r.jionschema + '.' + r.jiontable,
                                                  ForeignQuerySourceName = r.foreignschema + '.' + r.foreigntable,
                                                  JoinFieldName = r.jioncolumn,
                                                  ForeignFieldName = r.foreigncolumn
                                              })
                                              .ToList();

                return relationships;
            }
        }

        public DBSource LoadSchema(string connectionString)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var querySourceCategories = GetSchemas(conn, EXCLUDE_SCHEMAS);

                foreach (var category in querySourceCategories)
                {
                    category.QuerySources = new List<QuerySource>();

                    // Load Tables
                    category.QuerySources.AddRange(GetTables(conn, category.Name, SQL_TYPE_TABLE));

                    // Load Views
                    category.QuerySources.AddRange(GetTables(conn, category.Name, SQL_TYPE_VIEW));

                    // Load Functions
                    category.QuerySources.AddRange(GetFunctions(conn, category.Name));

                    // Load store procedure
                    category.QuerySources.AddRange(GetProcedures(conn, category.Name));

                    // Sort by name                    
                    category.QuerySources = category.QuerySources.OrderBy(s => s.Name).ToList();
                }

                return new DBSource
                {
                    QuerySources = querySourceCategories.ToList()
                };
            }
        }

        protected IList<QuerySourceCategory> GetSchemas(IDbConnection conn, string excludeSchemas)
        {
            string sql = string.Format(@"SELECT nspname FROM pg_namespace WHERE nspname NOT IN({0})", excludeSchemas);

            var querySourceCategories = conn.Query<dynamic>(sql)
                                          .Select(s => new QuerySourceCategory { Name = s.nspname })
                                          .ToList();

            return querySourceCategories;
        }

        protected IList<QuerySource> GetTables(IDbConnection conn, string schemaName, string tableType)
        {
            string sql = @" SELECT table_name,table_type
                            FROM information_schema.tables
                            WHERE table_schema=@schema AND table_type=@type";

            var sqlQuerySourceType = "UNKNOW";
            if (SQL_TYPE_TABLE.Equals(tableType, StringComparison.OrdinalIgnoreCase))
            {
                sqlQuerySourceType = SQLQuerySourceType.Table;
            }
            else if (SQL_TYPE_VIEW.Equals(tableType, StringComparison.OrdinalIgnoreCase))
            {
                sqlQuerySourceType = SQLQuerySourceType.View;
            }

            var querySources = conn.Query<dynamic>(sql, new { schema = schemaName, type = tableType })
                                          .Select(s => new QuerySource
                                          {
                                              Name = s.table_name,
                                              Type = sqlQuerySourceType
                                          })
                                          .ToList();

            return querySources;
        }

        protected IList<QuerySource> GetFunctions(IDbConnection conn, string schemaName)
        {
            string sql = string.Format(LOAD_SCHEMA_FUNCTION, 0);

            var querySources = conn.Query<dynamic>(sql, new { schema = schemaName, type = SQL_TYPE_FUNCTION });

            var result = new List<dynamic>();

            foreach (var querySource in querySources)
            {
                result.Add(querySource);
            }

            return result.Select(s => new QuerySource
            {
                Name = s.routine_name,
                Type = SQLQuerySourceType.Function
            }).GroupBy(x => x.Name).Select(x => x.First()).ToList();
        }

        protected IList<QuerySource> GetProcedures(IDbConnection conn, string schemaName)
        {
            string sql = string.Format(LOAD_SCHEMA_FUNCTION, 1);

            var querySources = conn.Query<dynamic>(sql, new { schema = schemaName, type = SQL_TYPE_FUNCTION });

            var result = querySources.Select(s => new QuerySource
            {
                Name = s.routine_name,
                Type = SQLQuerySourceType.Procedure
            }).GroupBy(x => x.Name);

            if (result.Where(x => x.Count() > 1).Count() > 0)
            {
                var modelErrors = new ModelErrors();
                modelErrors.AddError(string.Empty, Messages.DuplicateFunctionName);
                throw new IzendaModelException(modelErrors);
            }

            return result.Select(x => x.First()).ToList();
        }
    }
}
