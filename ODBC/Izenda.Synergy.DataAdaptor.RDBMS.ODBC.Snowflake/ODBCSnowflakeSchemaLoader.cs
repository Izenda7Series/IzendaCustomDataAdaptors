// ---------------------------------------------------------------------- 
// <copyright file="ODBCSnowflakeSchemaLoader.cs" company="Izenda">
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

using Izenda.BI.Framework.CustomAttributes;
using Izenda.Synergy.DataAdaptor.RDBMS.ODBC;
using Izenda.BI.DataAdaptor.RDBMS.Constants;
using Izenda.BI.DataAdaptor.RDBMS.ODBCSnowflake.Constants;
using Izenda.BI.Framework.Models;
using System.Collections.Generic;
using Izenda.BI.Logging;
using System.Data.Odbc;
using Dapper;
using System.Linq;
using System.Text.RegularExpressions;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBCSnowflake
{
    [DBServerTypeSupporting("88F5A470-9269-499B-A69C-70C94803AC3E", "ODBC Snowflake", "[ODBC] Snowflake")]
    public class ODBCSnowflakeSchemaLoader : ODBCSchemaLoader
    {
        public const string FOREIGN_KEY_CONSTRAINT_REGEX_PATTERN = @"constraint(.*)foreign key(.*)references(.*)";

        public override DatabaseSupportDataType DatabaseSupportDataType => new SnowflakeSupportDataType();

        public override List<Relationship> LoadRelationships(string connectionString, List<string> schemas = null)
        {
            using (var connection = new OdbcConnection(connectionString))
            {
                connection.Open();

                const string getTableQuery = @"SELECT DISTINCT TC.TABLE_CATALOG, TC.TABLE_SCHEMA, TC.TABLE_NAME FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC
                                INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC ON RC.CONSTRAINT_NAME = TC.CONSTRAINT_NAME";

                var relationships = new List<Relationship>();
                var tables = connection.Query<dynamic>(getTableQuery);
                foreach (var table in tables)
                {
                    // Get table DDL
                    var tableDDL = connection.Query<string>($"select get_ddl('table', '{table.TABLE_CATALOG}.{table.TABLE_SCHEMA}.{table.TABLE_NAME}');").First();

                    foreach (var line in tableDDL.ToLower().Split(','))
                    {
                        if (!Regex.IsMatch(line, FOREIGN_KEY_CONSTRAINT_REGEX_PATTERN, RegexOptions.IgnoreCase))
                        {
                            continue;
                        }

                        var joinSchema = table.TABLE_SCHEMA;
                        var joinTable = table.TABLE_NAME;
                        //Get join column
                        var fkStartIndex = line.IndexOf("foreign key") + "foreign key".Length;
                        var fkEndIndex = line.IndexOf("references");
                        var fkSets = line.Substring(fkStartIndex, fkEndIndex - fkStartIndex).Trim();
                        var joinColumn = fkSets.Substring(fkSets.IndexOf("(") + 1, fkSets.IndexOf(")") - fkSets.IndexOf("(") - 1);

                        // Get foriegn table and column
                        var ftStartIndex = line.IndexOf("references") + "references".Length;
                        var ftEndIndex = line.Length;

                        var refDefine = line.Substring(ftStartIndex, ftEndIndex - ftStartIndex);
                        var foreignTableFull = refDefine.Substring(0, refDefine.IndexOf("("));
                        var foreignSchema = foreignTableFull.Split('.')[1];
                        var foreignTable = foreignTableFull.Split('.')[2];

                        var foreignColumn = refDefine.Substring(refDefine.IndexOf("(") + 1, refDefine.IndexOf(")") - refDefine.IndexOf("(") - 1);

                        relationships.Add(new Relationship
                        {
                            JoinQuerySourceName = $"{joinSchema}.{joinTable}",
                            ForeignQuerySourceName = $"{foreignSchema}.{foreignTable}",
                            JoinFieldName = joinColumn,
                            ForeignFieldName = foreignColumn,
                        });
                    }
                }

                return relationships;
            }
        }

        protected override List<QuerySourceField> LoadFieldsFromProcedure(string connectionString, string type, string categoryName, string querySourceName, List<QuerySourceParameter> parameters = null, bool ignoreError = true, ILog log = null)
        {
            //Return empty list because Snowflake does not have conception of store procedure.
            return new List<QuerySourceField>();
        }
    }
}