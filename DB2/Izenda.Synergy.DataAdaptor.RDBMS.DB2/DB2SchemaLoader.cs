// ---------------------------------------------------------------------- 
// <copyright file="OracleConnection.cs" company="Izenda">
//  Copyright (c) 2015 Izenda, Inc.                          
//  ALL RIGHTS RESERVED                
//                                                                         
//  The entire contents of this file is protected by U.S. and      
//  International Copyright Laws. Unauthorized reproduction,        
//  reverse-engineering, and distribution of all or any portion of  
//  the code contained in this file is strictly prohibited and may  
//  result in severe civil and criminal penalties and will be      
//  prosecuted to the maximum extent possible under the law.        
//                                                                  
//  RESTRICTIONS                                                    
//                                                                  
//  THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES          
//  ARE CONFIDENTIAL AND PROPRIETARY TRADE                          
//  SECRETS OF IZENDA INC. THE REGISTERED DEVELOPER IS  
//  LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    
//  CONTROLS AS PART OF AN EXECUTABLE PROGRAM ONLY.                
//                                                                  
//  THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      
//  FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        
//  COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE      
//  AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  
//  AND PERMISSION FROM IZENDA INC.                      
//                                                                  
//  CONSULT THE END USER LICENSE AGREEMENT(EULA) FOR INFORMATION ON  
//  ADDITIONAL RESTRICTIONS.
// </copyright> 

using Izenda.BI.DataAdaptor.SQL.SchemaLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Izenda.BI.Framework.Models;
using Izenda.BI.Framework.CustomAttributes;
using IBM.Data.DB2;
using Dapper;
using Izenda.BI.RDBMS.Constants;
using Izenda.BI.DataAdaptor.RDBMS.DB2.Constants;
using Izenda.BI.Framework.Utility;
using System.Data;
using Izenda.BI.RDBMS.Model;
using System.Transactions;
using Izenda.BI.Framework.Constants;
using Izenda.BI.Framework.Exceptions;

namespace Izenda.Synergy.DataAdaptor.RDBMS.DB2
{
    [DBServerTypeSupporting("BFA5C22A-F69F-4766-A4C0-A5705EEAD545", "DB2", "[DB2] IBM DB2")]
    public class DB2SchemaLoader : ISchemaLoader
    {
        protected const string ExcludeSchemas = @"'NULLID', 'SQLJ', 'SYSCAT', 'SYSFUN', 'SYSIBM', 'SYSIBMADM', 'SYSIBMINTERNAL', 'SYSIBMTS', 'SYSPROC', 'SYSPUBLIC', 'SYSSTAT', 'SYSTOOLS'";

        public List<QuerySourceParameter> GetQuerySourceParameters(string connectionString)
        {
            return GetQuerySourceParameters(connectionString, string.Empty, string.Empty);
        }

        public List<QuerySourceParameter> GetQuerySourceParameters(string connectionString, string specificSchema, string specificName)
        {
            var dataTypeAdaptor = new DB2SupportDataType();

            using (var conn = new DB2Connection(connectionString))
            {
                const string sqlFormat = @"SELECT PARMNAME, ROUTINENAME, ROUTINESCHEMA, TYPENAME, ROWTYPE, ORDINAL 
                            FROM SYSIBM.SYSROUTINEPARMS
                            WHERE ROUTINESCHEMA NOT IN({0})";

                var sql = string.Format(sqlFormat, ExcludeSchemas);

                if (!string.IsNullOrEmpty(specificSchema) && !string.IsNullOrEmpty(specificName))
                {
                    sql += string.Format(" AND ROUTINESCHEMA = '{0}' AND ROUTINENAME = '{1}';", specificSchema, specificName);
                }
                else
                {
                    sql += ";";
                }

                var parametes =
                    conn.Query<dynamic>(sql)
                        .Select(s => new QuerySourceParameter
                        {
                            Name = string.Empty + s.PARMNAME,
                            QuerySourceName = s.ROUTINENAME,
                            Category = s.ROUTINESCHEMA,
                            DataType = s.TYPENAME,
                            IzendaDataType = dataTypeAdaptor.GetIzendaDataType(s.TYPENAME),
                            InputMode = s.ROWTYPE.Equals("P", StringComparison.OrdinalIgnoreCase),
                            Result = s.ROWTYPE.Equals("R", StringComparison.OrdinalIgnoreCase),
                            Position = s.ORDINAL,
                            Value = DBNull.Value,
                            AllowDistinct = dataTypeAdaptor.GetAllowDistinct(s.TYPENAME)
                        }).ToList();

                return parametes;
            }
        }

        public List<QuerySourceField> LoadCustomQuerySourceFields(string connectionString, string customQueryDefinition)
        {
            var result = new List<QuerySourceField>();
            var dataTypeAdaptor = new DB2SupportDataType();

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    using (var conn = new DB2Connection(connectionString))
                    {
                        conn.Open();

                        var command = new DB2Command(customQueryDefinition, conn);
                        command.CommandType = CommandType.Text;
                        var reader = command.ExecuteReader(CommandBehavior.SchemaOnly);
                        var schema = reader.GetSchemaTable();

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
                }
            }
            catch (DB2Exception ex)
            {
                var errorMsgBuilder = new StringBuilder();
                var modelErrors = new ModelErrors();
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    var error = ex.Errors[i];
                    errorMsgBuilder.AppendLine(error.Message);
                }

                modelErrors.AddError("CustomDefinition", errorMsgBuilder.ToString());
                throw new IzendaModelException(modelErrors);
            }

            return result;
        }

        public List<QuerySourceField> LoadFields(string connectionString, string type, string categoryName, string querySourceName, bool rollbackSP, List<QuerySourceParameter> parameters = null, bool ignoreError = true, int commandTimeout = 500, BI.Logging.ILog log = null)
        {
            var result = new List<QuerySourceField>();

            if (type.EqualsIgnoreCase(SQLQuerySourceType.Table) || type.EqualsIgnoreCase(SQLQuerySourceType.View))
            {
                result = LoadFieldsFromTable(connectionString, categoryName, querySourceName);
            }
            else if (type.EqualsIgnoreCase(SQLQuerySourceType.Procedure))
            {
                result = LoadFieldsFromProcedure(connectionString, type, categoryName, querySourceName, parameters, ignoreError, log);
            }

            return result;
        }

        public List<QuerySourceField> LoadQuerySourceFields(string connectionString)
        {
            return LoadFieldsFromTable(connectionString);
        }

        private List<QuerySourceField> LoadFieldsFromTable(string connectionString, string schemaName = null, string tableName = null)
        {
            var result = new List<QuerySourceField>();
            var dataTypeAdaptor = new DB2SupportDataType();

            using (var conn = new DB2Connection(connectionString))
            {
                conn.Open();

                var builder = new SchemaRestrictionsBuilder(true);
                if (!string.IsNullOrEmpty(schemaName))
                {
                    builder = builder.TableSchema(schemaName);
                }
                if (!string.IsNullOrEmpty(tableName))
                {
                    builder = builder.Table(tableName);
                }

                string[] restrictions = builder.Build();

                var columns = conn.GetSchema(DB2MetaDataCollectionNames.Columns, restrictions);

                //var columns = conn.GetSchema("Columns");

                foreach (var row in columns.Rows.OfType<DataRow>())
                {
                    var fieldDataType = string.Empty + row["DATA_TYPE_NAME"];
                    var izendaDataType = dataTypeAdaptor.GetIzendaDataType(fieldDataType);

                    if (string.IsNullOrEmpty(izendaDataType))
                    {
                        continue;
                    }

                    result.Add(
                        new QuerySourceField
                        {
                            Name = row["COLUMN_NAME"].ToString(),
                            DataType = fieldDataType,
                            IzendaDataType = izendaDataType,
                            AllowDistinct = dataTypeAdaptor.GetAllowDistinct(fieldDataType),
                            //ExtendedProperties = string.IsNullOrEmpty("" + row["column_key"]) ? "" : new FieldExtendedProperty { PrimaryKey = true }.ToJson(),//TODO FieldExtendedProperty ?
                            Position = Convert.ToInt32(row["ORDINAL_POSITION"]),
                            QuerySourceName = row["TABLE_NAME"].ToString(),
                            CategoryName = row["TABLE_SCHEMA"].ToString()
                        });
                }
            }

            return result;
        }

        /// <summary>
        /// Executes for schema.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="type">The type.</param>
        /// <param name="categoryName">Name of the schema.</param>
        /// <param name="querySourceName">Name of the specific.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// list of QuerySourceField
        /// </returns>
        private List<QuerySourceField> LoadFieldsFromProcedure(string connectionString, string type, string categoryName, string querySourceName, List<QuerySourceParameter> parameters = null, bool ignoreError = true, BI.Logging.ILog log = null)
        {
            var result = new List<QuerySourceField>();
            var dataTypeAdaptor = new DB2SupportDataType();

            if (parameters == null)
            {
                parameters = GetQuerySourceParameters(connectionString, categoryName, querySourceName);
            }

            parameters = parameters.OrderBy(x => x.Position).ToList();

            using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                using (var connection = new DB2Connection(connectionString))
                {
                    connection.Open();
                    string sql = String.Format("{0}.{1}", categoryName, querySourceName);
                    DB2Command cmd = new DB2Command(sql, connection);
                    if (type == SQLQuerySourceType.Procedure)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                    }

                    if (parameters != null && parameters.Count() > 0)
                    {
                        foreach (var parameter in parameters)
                        {
                            var db2Parameter = cmd.Parameters.Add(parameter.Name, parameter.Value);
                        }
                    }

                    try
                    {

                        var reader = cmd.ExecuteReader();
                        DataTable schema = reader.GetSchemaTable();

                        if (schema != null)
                        {
                            var colNames = schema.Rows.OfType<DataRow>().FirstOrDefault().Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                            var colStr = string.Join(", ", colNames.ToArray());

                            result.AddRange(schema
                                     .Rows
                                     .OfType<DataRow>()
                                     .Select
                                     (
                                        (c, i) => new QuerySourceField
                                        {
                                            Id = Guid.NewGuid(),
                                            GroupPosition = 1,
                                            Position = i,
                                            Name = c["ColumnName"].ToString(),
                                            DataType = reader.GetDataTypeName(int.Parse(c["ColumnOrdinal"].ToString())),
                                            IzendaDataType = dataTypeAdaptor.GetIzendaDataType(reader.GetDataTypeName(int.Parse(c["ColumnOrdinal"].ToString()))),
                                            AllowDistinct = dataTypeAdaptor.GetAllowDistinct(reader.GetDataTypeName(int.Parse(c["ColumnOrdinal"].ToString()))),
                                            Type = (int)QuerySourceFieldType.Field
                                        }
                                     )
                                     .Where(x => !string.IsNullOrEmpty(x.IzendaDataType)));
                        }

                        reader.Close();

                        return result;
                    }
                    catch (Exception ex)
                    {
                        log?.Debug(ex);

                        // ignore error when execute stored proc from customer connectionString
                        if (ignoreError)
                        {
                            return result;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
        }
        public List<Relationship> LoadRelationships(string connectionString, List<string> schemas = null)
        {
            using (var conn = new DB2Connection(connectionString))
            {
                string sql = $@"SELECT CONSTNAME, TABSCHEMA, TABNAME, FK_COLNAMES, REFTABSCHEMA, REFTABNAME, PK_COLNAMES FROM SYSCAT.REFERENCES;";

                var relationships = conn.Query<dynamic>(sql)
                                              .Select(r => new Relationship
                                              {
                                                  JoinQuerySourceName = r.TABSCHEMA + '.' + r.TABNAME,
                                                  ForeignQuerySourceName = r.REFTABSCHEMA + '.' + r.REFTABNAME,
                                                  JoinFieldName = r.FK_COLNAMES,
                                                  ForeignFieldName = r.PK_COLNAMES
                                              })
                                              .ToList();

                return relationships;
            }
        }

        public DBSource LoadSchema(string connectionString)
        {
            using (var conn = new DB2Connection(connectionString))
            {
                conn.Open();

                var querySourceCategories = GetSchemas(conn, ExcludeSchemas);

                foreach (var category in querySourceCategories)
                {
                    category.QuerySources = new List<QuerySource>();

                    // Load Tables
                    category.QuerySources.AddRange(GetTables(conn, category.Name));

                    // Load Views
                    category.QuerySources.AddRange(GetViews(conn, category.Name));

                    // Load Procedures
                    category.QuerySources.AddRange(GetProcedures(conn, category.Name));

                    // Load Functions
                    category.QuerySources.AddRange(GetFunctions(conn, category.Name));

                    // Sort by name
                    category.QuerySources = category.QuerySources.OrderBy(s => s.Name).ToList();
                }

                return new DBSource
                {
                    QuerySources = querySourceCategories.ToList()
                };
            }
        }

        protected IList<QuerySourceCategory> GetSchemas(DB2Connection conn, string excludeSchemas)
        {
            string sql = string.Format(@"SELECT SCHEMANAME 
                                            FROM SYSCAT.SCHEMATA 
                                            WHERE SCHEMANAME NOT IN({0});", excludeSchemas);

            var querySourceCategories = conn.Query<dynamic>(sql)
                                          .Select(s => new QuerySourceCategory { Name = s.SCHEMANAME })
                                          .ToList();

            return querySourceCategories;

            //var tables = conn.GetSchema(DB2MetaDataCollectionNames.Schemas);
            //var result = new List<QuerySourceCategory>();
            //foreach (var row in tables.Rows.OfType<DataRow>())
            //{
            //    var name = row["TABLE_SCHEMA"].ToString();
            //    if (!excludeSchemas.Contains(name))
            //    {
            //        result.Add(
            //        new QuerySourceCategory
            //        {
            //            Name = name
            //        });
            //    }
            //}
            //return result;
        }

        /// <summary>
        /// Get query source
        /// </summary>
        private static IList<QuerySource> GetQuerySources(DataTable tables, string sqlQuerySourceType, string dataColumnName)
        {
            var result = new List<QuerySource>();
            foreach (var row in tables.Rows.OfType<DataRow>())
            {
                result.Add(
                    new QuerySource
                    {
                        Name = row[dataColumnName].ToString(),
                        Type = sqlQuerySourceType
                    });
            }
            return result;
        }

        private IEnumerable<QuerySource> GetTables(DB2Connection conn, string schemaName)
        {
            string[] restrictions = new SchemaRestrictionsBuilder()
                .TableSchema(schemaName)
                .TableType("TABLE")
                .Build();
            var tables = conn.GetSchema(DB2MetaDataCollectionNames.Tables, restrictions);

            return GetQuerySources(tables, SQLQuerySourceType.Table, "TABLE_NAME");

            //string sql = string.Format(@"SELECT NAME FROM SYSIBM.SYSTABLES WHERE TYPE = 'T' AND CREATOR = '{0}';", schemaName);

            //var result = conn.Query<dynamic>(sql)
            //    .Select(s => new QuerySource
            //    {
            //        Name = s.NAME,
            //        Type = SQLQuerySourceType.Table
            //    }).ToList();

            //return result;
        }

        private IEnumerable<QuerySource> GetViews(DB2Connection conn, string schemaName)
        {
            string[] restrictions = new SchemaRestrictionsBuilder()
                .TableSchema(schemaName)
                .TableType("View")
                .Build();
            var tables = conn.GetSchema(DB2MetaDataCollectionNames.Tables, restrictions);

            return GetQuerySources(tables, SQLQuerySourceType.View, "TABLE_NAME");

            //string sql = string.Format(@"SELECT NAME FROM SYSIBM.SYSTABLES WHERE TYPE = 'V' AND CREATOR = '{0}';", schemaName);

            //var result = conn.Query<dynamic>(sql)
            //    .Select(s => new QuerySource
            //    {
            //        Name = s.NAME,
            //        Type = SQLQuerySourceType.View
            //    }).ToList();

            //return result;
        }

        private IEnumerable<QuerySource> GetProcedures(DB2Connection conn, string schemaName)
        {
            string[] restrictions = new SchemaRestrictionsBuilder()
                .TableSchema(schemaName)
                .Build();

            var tables = conn.GetSchema(DB2MetaDataCollectionNames.Procedures, restrictions);

            return GetQuerySources(tables, SQLQuerySourceType.Procedure, "PROCEDURE_NAME");

            //string sql = string.Format(@"SELECT PROCNAME FROM SYSCAT.PROCEDURES WHERE PROCSCHEMA = '{0}';", schemaName);

            //var result = conn.Query<dynamic>(sql)
            //    .Select(s => new QuerySource
            //    {
            //        Name = s.PROCNAME,
            //        Type = SQLQuerySourceType.Procedure
            //    }).ToList();

            //return result;
        }

        private IEnumerable<QuerySource> GetFunctions(DB2Connection conn, string schemaName)
        {
            string sql = string.Format(@"SELECT FUNCNAME FROM SYSCAT.FUNCTIONS WHERE FUNCSCHEMA = '{0}';", schemaName);

            var result = conn.Query<dynamic>(sql)
                .Select(s => new QuerySource
                {
                    Name = s.FUNCNAME,
                    Type = SQLQuerySourceType.Function
                }).ToList();

            return result;
        }

    }
}
