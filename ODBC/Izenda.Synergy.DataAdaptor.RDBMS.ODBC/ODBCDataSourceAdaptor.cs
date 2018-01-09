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

using Izenda.BI.DataAdaptor.RDBMS;
using System;
using System.Collections.Generic;
using Izenda.BI.DataAdaptor.SQL.SchemaLoader;
using Izenda.BI.Framework;
using Izenda.BI.Framework.Constants;
using Izenda.BI.Framework.Models;
using Dapper;
using Izenda.BI.Framework.Exceptions;
using Izenda.BI.Resource;
using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.ODBC.CommandGenerators;
using System.Data.Odbc;

namespace Izenda.Synergy.DataAdaptor.RDBMS.ODBC
{
    public abstract class ODBCDataSourceAdaptor : DataSourceAdaptor
    {
        public abstract bool IsSupportedMultipleQuery { get; }

        public override IConnection Connection => new ODBCConnection();

        public override ISchemaLoader SchemaLoader => new ODBCSchemaLoader();

        /// <summary>
        /// The query tree command generator
        /// </summary>
        public override QueryTreeCommandGenerator QueryTreeCommandGenerator
        {
            get
            {
                return new ODBCQueryTreeCommandGenerator();
            }
        }

        /// <summary>
        /// The get first value query
        /// </summary>
        public override string GetFirstValueInFilteredQuery
        {
            get
            {
                throw new NotImplementedException("Get first value query has to override for each specific RDBMS");
            }
        }

        public override List<DatabaseDataType> GetBaseDataTypes()
        {
            throw new NotImplementedException("Have to override this method for each specific RDBMS");
        }

        public override string GetConnectionStringWithServerAndDatabaseName(string connectionString)
        {
            var builder = new OdbcConnectionStringBuilder(connectionString);
            return string.Format("Server={0};Database={1};", builder.Driver, builder.Driver);//TODO: ODBC connection string with server and db name
        }

        public override IEnumerable<T> Query<T>(string connectionString, string query, object param = null, int queryTimeout = 60)
        {
            query = NormalizeQuery(query);

            using (var connection = OpenConnection(connectionString))
            {
                try
                {
                    Log($"ODBC Execute Query: {query}", LogType.Info);
                    return connection.Query<T>(query, param, commandTimeout: queryTimeout);
                }
                catch (OdbcException ex)
                {
                    Log($"Query error: {ex.ToString()}. {Environment.NewLine}Query: {query}", LogType.Error);
                    throw new FusionException($"{Messages.FusionCanNotQueryData}{Environment.NewLine}Error Detail: {ex.Message}");
                }
            }
        }

        public override IEnumerable<T> QueryMultiple<T>(string connectionString, string query, object param = null, int queryTimeout = 60, Action<SqlMapper.GridReader> action = null)
        {
            if (IsSupportedMultipleQuery)
            {
                query = NormalizeQuery(query);

                using (var connection = OpenConnection(connectionString))
                {
                    try
                    {
                        using (var result = connection.QueryMultiple(query, param, commandTimeout: queryTimeout))
                        {
                            var returnResult = result.Read<T>();
                            action?.Invoke(result);
                            return returnResult;
                        }
                    }
                    catch (OdbcException ex)
                    {
                        Log($"Query error: {ex.ToString()}. {Environment.NewLine}Query: {query}", LogType.Error);
                        throw new FusionException($"{Messages.FusionCanNotQueryData}{Environment.NewLine}Error Detail: {ex.Message}");
                    }
                }
            }

            throw new NotSupportedException("Not support multiple query");
        }

        protected override IEnumerable<dynamic> GetPagingResult(string connectionString, string query, FusionContextData context)
        {
            if (IsSupportedMultipleQuery)
            {
                return base.GetPagingResult(connectionString, query, context);
            }

            // If not supported mutiple query, execute two query separately
            var result = Query<dynamic>(connectionString, query,
                context.Parameters, context.PerformanceSetting.QueryTimeoutValue);

            var countTotalQuery = CountTotalCommand(context);
            var totalRows = Query<int>(
                connectionString,
                countTotalQuery,
                context.Parameters,
                context.PerformanceSetting.QueryTimeoutValue).AsList()[0];

            context.Paging.Total = context.RowLimit > 0 ? Math.Min(context.RowLimit, totalRows) : totalRows;
            return result;
        }

        protected override string NormalizeQuery(string query)
        {
            throw new NotSupportedException("Have to provide specific query normalizer for each RDBMS");
        }
    }
}
