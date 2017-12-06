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
using System.ComponentModel.Composition;
using Izenda.BI.DataAdaptor;
using Izenda.BI.Framework.Constants;
using Izenda.BI.Framework.Models;
using Dapper;
using Izenda.BI.DataAdaptor.RDBMS.DB2.Constants;
using Izenda.BI.Framework.Components.SequenceWorkflows;
using Izenda.BI.Framework.Models.Contexts;
using Izenda.BI.QueryNormalizer.DB2;
using IBM.Data.DB2;
using Izenda.BI.Framework.Exceptions;
using Izenda.BI.Resource;
using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.DB2.CommandGenerators;

namespace Izenda.Synergy.DataAdaptor.RDBMS.DB2
{
    [Export(typeof(IDataSourceAdaptor))]
    [ExportMetadata("ServerType", "1D29A37C-2B4F-4A6C-8557-35CBCA3DC8A1|DB2|[DB2] IBM DB2")]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class DB2DataSourceAdaptor : DataSourceAdaptor
    {
        /// <summary>
        /// The query normalizer
        /// </summary>
        ISequenceWorkflow<DB2QueryNormalizerActivity, QueryNormalizerContext> queryNormalizer;

        /// <summary>
        /// The query normalizer
        /// </summary>
        protected ISequenceWorkflow<DB2QueryNormalizerActivity, QueryNormalizerContext> QueryNormalizer
        {
            get
            {
                if (queryNormalizer == null)
                {
                    queryNormalizer = new SequenceWorkflow<DB2QueryNormalizerActivity, QueryNormalizerContext>();
                }

                return queryNormalizer;
            }
        }

        public override IConnection Connection => new IBMDB2Connection();

        public override ISchemaLoader SchemaLoader => new DB2SchemaLoader();

        /// <summary>
        /// The query tree command generator
        /// </summary>
        public override QueryTreeCommandGenerator QueryTreeCommandGenerator
        {
            get
            {
                return new DB2QueryTreeCommandGenerator();
            }
        }

        /// <summary>
        /// The get first value query
        /// </summary>
        public override string GetFirstValueInFilteredQuery
        {
            get
            {
                return @"SELECT {0} FROM {1} FETCH FIRST 1 ROW ONLY";
            }
        }

        public override List<DatabaseDataType> GetBaseDataTypes()
        {
            var dataTypeAdapter = new DB2SupportDataType();
            return dataTypeAdapter.GetBaseDataTypes();
        }

        public override string GetConnectionStringWithServerAndDatabaseName(string connectionString)
        {
            var builder = new DB2ConnectionStringBuilder(connectionString);
            return string.Format("Server={0};Database={1};", builder.Server, builder.Database);
        }

        public override IEnumerable<T> Query<T>(string connectionString, string query, object param = null, int queryTimeout = 60)
        {
            query = NormalizeQuery(query);

            using (var connection = OpenConnection(connectionString))
            {
                try
                {
                    return connection.Query<T>(query, param, commandTimeout: queryTimeout);
                }
                catch (DB2Exception ex)
                {
                    Log("Query error: " + ex.ToString(), LogType.Error);
                    throw new FusionException($"{Messages.FusionCanNotQueryData}{Environment.NewLine}Error Detail: {ex.Message}");
                }
            }
        }

        public override IEnumerable<T> QueryMultiple<T>(string connectionString, string query, object param = null, int queryTimeout = 60, Action<SqlMapper.GridReader> action = null)
        {
            query = NormalizeQuery(query);

            using (var connection = OpenConnection(connectionString))
            {
                try
                {
                    using (var data = connection.QueryMultiple(query, param, commandTimeout: queryTimeout))
                    {
                        var result = data.Read<T>();
                        action?.Invoke(data);
                        return result;
                    }
                }
                catch (DB2Exception ex)
                {
                    Log("Query error: " + ex.ToString(), LogType.Error);
                    throw new FusionException($"{Messages.FusionCanNotQueryData}{Environment.NewLine}Error Detail: {ex.Message}");
                }
            }
        }

        protected override string NormalizeQuery(string query)
        {
            var normalizerContext = new QueryNormalizerContext { Query = query };
            QueryNormalizer.Execute(normalizerContext);
            return normalizerContext.Query;
        }
    }
}
