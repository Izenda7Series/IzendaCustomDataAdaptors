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
using Izenda.BI.RDBMS.Constants;
using Izenda.BI.Framework.Utility;
using System.Data;
using Izenda.BI.Framework.Exceptions;
using System.Data.Odbc;
using Izenda.BI.DataAdaptor.RDBMS.Constants;
using System.Transactions;
using Izenda.BI.Framework.Constants;

namespace Izenda.Synergy.DataAdaptor.RDBMS.ODBC
{
    [DBServerTypeSupporting("88F5A470-9269-499B-A69C-70C94803FC3E", "ODBC", "[ODBC] ODBC")]
    public class ODBCSchemaLoader : ISchemaLoader
    {
        public virtual DatabaseSupportDataType DatabaseSupportDataType
        {
            get
            {
                throw new NotImplementedException("Have to provide overriding for DatabaseSupportDataType for specific RDBMS");
            }
        }

        public virtual DBSource LoadSchema(string connectionString)
        {
            using (var conn = new OdbcConnection(connectionString))
            {
                conn.Open();

                var querySourceCategories = GetSchemas(conn);

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

        public virtual List<QuerySourceField> LoadFields(string connectionString, string type, string categoryName, string querySourceName, bool rollbackSP, List<QuerySourceParameter> parameters = null, bool ignoreError = true, int commandTimeout = 500, BI.Logging.ILog log = null)
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

        public virtual List<QuerySourceField> LoadCustomQuerySourceFields(string connectionString, string customQueryDefinition)
        {
            var result = new List<QuerySourceField>();

            try
            {
                //using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew))//UNDONE: workaround ODBC transaction issue
                //{
                using (var conn = new OdbcConnection(connectionString))
                {
                    conn.Open();

                    var command = new OdbcCommand(customQueryDefinition, conn);
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
                                IzendaDataType = DatabaseSupportDataType.GetIzendaDataType(dataType),
                                AllowDistinct = DatabaseSupportDataType.GetAllowDistinct(dataType),
                                ExtendedProperties = "",
                                Position = Convert.ToInt32(schema.Rows[0]["ColumnOrdinal"].ToString())
                            }
                        );
                    }
                }
                //}
            }
            catch (OdbcException ex)
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

        public virtual List<QuerySourceField> LoadQuerySourceFields(string connectionString)
        {
            return LoadFieldsFromTable(connectionString);
        }

        public virtual List<Relationship> LoadRelationships(string connectionString, List<string> schemas = null)
        {
            throw new NotImplementedException("ODBC Connection does not have mechanism to load relationship. You have to override this method to provide relationship loading for specific RDBMS");
        }

        public virtual List<QuerySourceParameter> GetQuerySourceParameters(string connectionString)
        {
            return GetQuerySourceParameters(connectionString, string.Empty, string.Empty);
        }

        public virtual List<QuerySourceParameter> GetQuerySourceParameters(string connectionString, string specificSchema, string specificName)
        {
            using (var conn = new OdbcConnection(connectionString))
            {
                conn.Open();

                // Build ODBC Schema Restriction for ProcedureParameters
                var builder = new OdbcConnectionStringBuilder(conn.ConnectionString);

                var restrictions = new string[4];
                restrictions[0] = builder["database"] + string.Empty;
                if (!string.IsNullOrEmpty(specificSchema))
                {
                    restrictions[1] = specificSchema;
                }
                if (!string.IsNullOrEmpty(specificName))
                {
                    restrictions[2] = specificName;
                }
                var procedureParameters = conn.GetSchema(OdbcMetaDataCollectionNames.ProcedureParameters, restrictions);

                var querySourceParameters = new List<QuerySourceParameter>();
                foreach (var row in procedureParameters.Rows.OfType<DataRow>())
                {
                    var possition = -1;
                    int.TryParse(string.Empty + row["ORDINAL_POSITION"], out possition);

                    var coulumnType = 0;
                    int.TryParse(string.Empty + row["COLUMN_TYPE"], out coulumnType);//0: unknow, 1: input, 2: input/output, 3: output, 4: return result, 5: result col

                    var parameter = new QuerySourceParameter
                    {
                        Name = string.Empty + row["COLUMN_NAME"],
                        QuerySourceName = string.Empty + row["PROCEDURE_NAME"],
                        Category = string.Empty + row["PROCEDURE_SCHEM"],
                        DataType = string.Empty + row["TYPE_NAME"],
                        IzendaDataType = DatabaseSupportDataType.GetIzendaDataType(string.Empty + row["TYPE_NAME"]),
                        InputMode = coulumnType == 1 || coulumnType == 3,
                        Result = coulumnType == 4 || coulumnType == 5,
                        Position = possition,
                        Value = DBNull.Value,
                        AllowDistinct = DatabaseSupportDataType.GetAllowDistinct(string.Empty + row["TYPE_NAME"])
                    };
                    querySourceParameters.Add(parameter);
                }

                return querySourceParameters;
            }
        }

        /// <summary>
        /// Load list of columns of table presents in <see cref="QuerySourceField"/> object. This method is called inside method <see cref="LoadFields(string, string, string, string, bool, List{QuerySourceParameter}, bool, int, BI.Logging.ILog)"/> in case the field type is <see cref="SQLQuerySourceType.Table"/>.
        /// This method also is called inside method <see cref="LoadQuerySourceFields(string)"/>.
        /// Override this method to customizes the way system loads table field as well as query source fields for ODBC connection.
        /// </summary>
        /// <param name="connectionString">The connection to database</param>
        /// <param name="schemaName">The schama name</param>
        /// <param name="tableName">The table name</param>
        /// <returns>The list of query source field, each query source field presents for a column name of table.</returns>
        protected virtual List<QuerySourceField> LoadFieldsFromTable(string connectionString, string schemaName = null, string tableName = null)
        {
            var result = new List<QuerySourceField>();

            using (var conn = new OdbcConnection(connectionString))
            {
                conn.Open();

                var builder = new OdbcConnectionStringBuilder(conn.ConnectionString);

                var restrictions = new string[4];
                restrictions[0] = builder["database"] + string.Empty;
                if (!string.IsNullOrEmpty(schemaName))
                {
                    restrictions[1] = schemaName;
                }
                if (!string.IsNullOrEmpty(tableName))
                {
                    restrictions[2] = tableName;
                }

                var columns = conn.GetSchema(OdbcMetaDataCollectionNames.Columns, restrictions);

                foreach (var row in columns.Rows.OfType<DataRow>())
                {
                    var fieldDataType = string.Empty + row["TYPE_NAME"];
                    var izendaDataType = DatabaseSupportDataType.GetIzendaDataType(fieldDataType);

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
                            AllowDistinct = DatabaseSupportDataType.GetAllowDistinct(fieldDataType),
                            //ExtendedProperties = string.IsNullOrEmpty("" + row["column_key"]) ? "" : new FieldExtendedProperty { PrimaryKey = true }.ToJson(),//TODO: FieldExtendedProperty ?
                            Position = Convert.ToInt32(row["ORDINAL_POSITION"]),
                            QuerySourceName = row["TABLE_NAME"].ToString(),
                            CategoryName = row["TABLE_SCHEM"].ToString()
                        });
                }
            }

            return result;
        }

        /// <summary>
        /// Load columns definition of store proceudre.
        /// This method is called inside <see cref="LoadFields(string, string, string, string, bool, List{QuerySourceParameter}, bool, int, BI.Logging.ILog)"/> in case field type is <see cref="SQLQuerySourceType.Procedure"/>
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="type">The field type <see cref="SQLQuerySourceType"/>.</param>
        /// <param name="categoryName">Name of the schema.</param>
        /// <param name="querySourceName">Name of the specific.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// List of query source field which are columns list of resut set of procedure.
        /// </returns>
        protected virtual List<QuerySourceField> LoadFieldsFromProcedure(string connectionString, string type, string categoryName, string querySourceName, List<QuerySourceParameter> parameters = null, bool ignoreError = true, BI.Logging.ILog log = null)
        {
            var result = new List<QuerySourceField>();

            if (parameters == null)
            {
                parameters = GetQuerySourceParameters(connectionString, categoryName, querySourceName);
            }

            parameters = parameters.OrderBy(x => x.Position).ToList();

            using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                using (var connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    string sql = String.Format("{0}.{1}", categoryName, querySourceName);
                    var cmd = new OdbcCommand(sql, connection);
                    if (type == SQLQuerySourceType.Procedure)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                    }

                    if (parameters != null && parameters.Count() > 0)
                    {
                        foreach (var parameter in parameters)
                        {
                            var ODBCParameter = cmd.Parameters.AddWithValue(parameter.Name, parameter.Value);
                        }
                    }

                    try
                    {

                        var reader = cmd.ExecuteReader();
                        DataTable schema = reader.GetSchemaTable();

                        if (schema != null)
                        {
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
                                            IzendaDataType = DatabaseSupportDataType.GetIzendaDataType(reader.GetDataTypeName(int.Parse(c["ColumnOrdinal"].ToString()))),
                                            AllowDistinct = DatabaseSupportDataType.GetAllowDistinct(reader.GetDataTypeName(int.Parse(c["ColumnOrdinal"].ToString()))),
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

        protected IList<QuerySourceCategory> GetSchemas(OdbcConnection connection)
        {
            var querySourceCategories = new List<QuerySourceCategory>();

            var builder = new OdbcConnectionStringBuilder(connection.ConnectionString);
            var dataBaseName = builder["database"] + string.Empty;

            var restrictions = new string[3];
            restrictions[0] = dataBaseName;

            var tables = connection.GetSchema(OdbcMetaDataCollectionNames.Tables, restrictions);
            foreach (var row in tables.Rows.OfType<DataRow>())
            {
                var schemaName = string.Empty + row["TABLE_SCHEM"];
                if (!querySourceCategories.Any(s => s.Name.Equals(schemaName)))
                {
                    querySourceCategories.Add(new QuerySourceCategory { Name = schemaName });
                }
            }

            return querySourceCategories;
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

        private IEnumerable<QuerySource> GetTables(OdbcConnection conn, string schemaName)
        {
            var builder = new OdbcConnectionStringBuilder(conn.ConnectionString);

            var restrictions = new string[3];
            restrictions[0] = builder["database"] + string.Empty;
            if (!string.IsNullOrEmpty(schemaName))
            {
                restrictions[1] = schemaName;
            }

            var tables = conn.GetSchema(OdbcMetaDataCollectionNames.Tables, restrictions);

            return GetQuerySources(tables, SQLQuerySourceType.Table, "TABLE_NAME");
        }

        private IEnumerable<QuerySource> GetViews(OdbcConnection conn, string schemaName)
        {
            var builder = new OdbcConnectionStringBuilder(conn.ConnectionString);

            var restrictions = new string[3];
            restrictions[0] = builder["database"] + string.Empty;
            if (!string.IsNullOrEmpty(schemaName))
            {
                restrictions[1] = schemaName;
            }

            var tables = conn.GetSchema(OdbcMetaDataCollectionNames.Views, restrictions);

            return GetQuerySources(tables, SQLQuerySourceType.View, "TABLE_NAME");
        }

        private IEnumerable<QuerySource> GetProcedures(OdbcConnection conn, string schemaName)
        {
            var builder = new OdbcConnectionStringBuilder(conn.ConnectionString);

            var restrictions = new string[4];
            restrictions[0] = builder["database"] + string.Empty;
            if (!string.IsNullOrEmpty(schemaName))
            {
                restrictions[1] = schemaName;
            }
            restrictions[3] = 1.ToString();
            var tables = conn.GetSchema(OdbcMetaDataCollectionNames.Procedures, restrictions);

            return GetQuerySources(tables, SQLQuerySourceType.Procedure, "PROCEDURE_NAME");
        }

        private IEnumerable<QuerySource> GetFunctions(OdbcConnection conn, string schemaName)
        {
            var builder = new OdbcConnectionStringBuilder(conn.ConnectionString);

            var restrictions = new string[4];
            restrictions[0] = builder["database"] + string.Empty;
            if (!string.IsNullOrEmpty(schemaName))
            {
                restrictions[1] = schemaName;
            }
            restrictions[3] = 2.ToString();

            var tables = conn.GetSchema(OdbcMetaDataCollectionNames.Procedures, restrictions);

            return GetQuerySources(tables, SQLQuerySourceType.Function, "PROCEDURE_NAME");
        }
    }
}
