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

using System.Collections.Generic;
using Izenda.BI.Framework.Models;
using Izenda.BI.Framework.Components.SequenceWorkflows;
using Izenda.BI.Framework.Models.Contexts;
using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.QueryNormalizer.ODBC;
using Izenda.BI.DataAdaptor.RDBMS.ODBCSnowflake.Constants;
using System.ComponentModel.Composition;
using Izenda.BI.DataAdaptor;
using Izenda.BI.DataAdaptor.RDBMS.ODBCSnowflake.CommandGenerators;
using Izenda.BI.DataAdaptor.SQL.SchemaLoader;
using Izenda.BI.DataAdaptor.RDBMS.ODBCSnowflake;
using Izenda.Synergy.DataAdaptor.RDBMS.ODBC;
using System;
using Izenda.BI.Framework.Constants;

namespace Izenda.Synergy.DataAdaptor.RDBMS.ODBCSnowflake
{
    [Export(typeof(IDataSourceAdaptor))]
    [ExportMetadata("ServerType", "60168F4D-A810-47AB-8EFE-564EF928C1EE| ODBC Snowflake|[ODBC] Snowflake")]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ODBCSnowflakeDataSourceAdaptor : ODBCDataSourceAdaptor
    {
        /// <summary>
        /// Indicate whether this RDBMS is supported or not
        /// </summary>
        public override bool IsSupportedMultipleQuery
        {
            get
            {
                return false;
            }
        }

        public override ISchemaLoader SchemaLoader => new ODBCSnowflakeSchemaLoader();

        /// <summary>
        /// The query normalizer
        /// </summary>
        ISequenceWorkflow<ODBCSnowflakeQueryNormalizerActivity, QueryNormalizerContext> redshiftQueryNormalizer;

        /// <summary>
        /// The query normalizer
        /// </summary>
        protected ISequenceWorkflow<ODBCSnowflakeQueryNormalizerActivity, QueryNormalizerContext> RedshiftQueryNormalizer
        {
            get
            {
                if (redshiftQueryNormalizer == null)
                {
                    redshiftQueryNormalizer = new SequenceWorkflow<ODBCSnowflakeQueryNormalizerActivity, QueryNormalizerContext>();
                }

                return redshiftQueryNormalizer;
            }
        }

        /// <summary>
        /// The supported operation
        /// </summary>
        protected override Dictionary<string, Version> SupportedOperations
        {
            get
            {
                var databaseOperations = base.SupportedOperations;
                databaseOperations.Remove(DatabaseOperations.SubTotal);

                return databaseOperations;
            }
        }

        /// <summary>
        /// The partial supported operation
        /// </summary>
        protected override Dictionary<string, Version> PartialSupportedOperations
        {
            get
            {
                var partialSupportedOperations = base.PartialSupportedOperations;
                partialSupportedOperations.Remove(DatabaseOperations.SubTotal);

                return partialSupportedOperations;
            }
        }

        /// <summary>
        /// The query tree command generator
        /// </summary>
        public override QueryTreeCommandGenerator QueryTreeCommandGenerator
        {
            get
            {
                return new ODBCSnowflakeQueryTreeCommandGenerator();
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
            var dataTypeAdaptor = new SnowflakeSupportDataType();
            return dataTypeAdaptor.GetBaseDataTypes();
        }

        protected override string NormalizeQuery(string query)
        {
            var normalizerContext = new QueryNormalizerContext { Query = query };
            RedshiftQueryNormalizer.Execute(normalizerContext);
            return normalizerContext.Query;
        }
    }
}
