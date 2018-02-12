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

namespace Izenda.BI.DataAdaptor.RDBMS.ODBCSnowflake
{
    [DBServerTypeSupporting("88F5A470-9269-499B-A69C-70C94803AC3E", "ODBC Snowflake", "[ODBC] Snowflake")]
    public class ODBCSnowflakeSchemaLoader : ODBCSchemaLoader
    {
        public override DatabaseSupportDataType DatabaseSupportDataType => new SnowflakeSupportDataType();

        public override List<Relationship> LoadRelationships(string connectionString, List<string> schemas = null)
        {
            //throw new System.NotImplementedException("Have to implement loading relationship for Snowflake database");//UNDONE: have to implement this method
            //using (var connection = new OdbcConnection(connectionString))
            //{
            //    connection.Open();

            //    var query = @"select kcu.constraint_name as fk_name,
            //                            kcu.constraint_schema as jionschema,
            //                            kcu.table_name as jiontable,
            //                            kcu.column_name as jioncolumn,
            //                            ccu.constraint_schema as foreignschema,
            //                            ccu.table_name as foreigntable,
            //                            ccu.column_name as foreigncolumn
            //                    from information_schema.key_column_usage as kcu
            //                    inner join information_schema.constraint_column_usage as ccu on ccu.constraint_name = kcu.constraint_name
            //                    inner join pg_constraint as pgc on pgc.conname = kcu.constraint_name
            //                    where pgc.contype = 'f'";

            //    var relationships = connection.Query<dynamic>(query)
            //        .Select(r => new Relationship
            //        {
            //            JoinQuerySourceName = r.jionschema + '.' + r.jiontable,
            //            ForeignQuerySourceName = r.foreignschema + '.' + r.foreigntable,
            //            JoinFieldName = r.jioncolumn,
            //            ForeignFieldName = r.foreigncolumn
            //        }).ToList();

            //    return relationships;
            //}

            return new List<Relationship>();
        }

        protected override List<QuerySourceField> LoadFieldsFromProcedure(string connectionString, string type, string categoryName, string querySourceName, List<QuerySourceParameter> parameters = null, bool ignoreError = true, ILog log = null)
        {
            //Return empty list because Snowflake does not have conception of store procedure.
            return new List<QuerySourceField>();
        }
    }
}
