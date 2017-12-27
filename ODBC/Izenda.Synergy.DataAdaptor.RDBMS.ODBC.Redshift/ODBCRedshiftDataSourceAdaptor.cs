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
using System.ComponentModel.Composition;
using Izenda.BI.Framework.Models;
using Izenda.BI.DataAdaptor.RDBMS.ODBC.Redshift.Constants;
using Izenda.BI.Framework.Components.SequenceWorkflows;
using Izenda.BI.Framework.Models.Contexts;
using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.Synergy.DataAdaptor.RDBMS.ODBC;
using Izenda.BI.DataAdaptor.RDBMS.ODBC.CommandGenerators;
using Izenda.BI.QueryNormalizer.ODBC.Redshift;
using Izenda.BI.Framework.Constants;
using System;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBC.Redshift
{
    [Export(typeof(IDataSourceAdaptor))]
    [ExportMetadata("ServerType", "751A82D4-6B28-406E-AE8E-88035E72D0A8|ODBC Redshift|[ODBC] Redshift")]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ODBCRedshiftDataSourceAdaptor : ODBCDataSourceAdaptor
    {
        /// <summary>
        /// Indicate whether this RDBMS is supported or not
        /// </summary>
        public override bool IsSupportedMultipleQuery
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// The query normalizer
        /// </summary>
        ISequenceWorkflow<ODBCRedshiftQueryNormalizerActivity, QueryNormalizerContext> redshiftQueryNormalizer;

        /// <summary>
        /// The query normalizer
        /// </summary>
        protected ISequenceWorkflow<ODBCRedshiftQueryNormalizerActivity, QueryNormalizerContext> RedshiftQueryNormalizer
        {
            get
            {
                if (redshiftQueryNormalizer == null)
                {
                    redshiftQueryNormalizer = new SequenceWorkflow<ODBCRedshiftQueryNormalizerActivity, QueryNormalizerContext>();
                }

                return redshiftQueryNormalizer;
            }
        }

        /// <summary>
        /// The query tree command generator
        /// </summary>
        public override QueryTreeCommandGenerator QueryTreeCommandGenerator
        {
            get
            {
                return new ODBCRedshiftQueryTreeCommandGenerator();
            }
        }

        /// <summary>
        /// The get first value query
        /// </summary>
        public override string GetFirstValueInFilteredQuery
        {
            get
            {
                return @"SELECT TOP 1 {0} FROM {1}";
            }
        }

        public override List<DatabaseDataType> GetBaseDataTypes()
        {
            var dataTypeAdaptor = new RedshiftSupportDataType();
            return dataTypeAdaptor.GetBaseDataTypes();
        }

        protected override string NormalizeQuery(string query)
        {
            var normalizerContext = new QueryNormalizerContext { Query = query };
            RedshiftQueryNormalizer.Execute(normalizerContext);
            return normalizerContext.Query;
        }

        /// <summary>
        /// The supported operation
        /// </summary>
        protected override Dictionary<string, Version> SupportedOperations
        {
            get
            {
                var databaseOperations = base.SupportedOperations;

                //Postgres only support subtotal from 9.5
                databaseOperations[DatabaseOperations.SubTotal] = new Version(9, 5);

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
                var databaseOperations = base.PartialSupportedOperations;

                //Postgres only support subtotal from 9.5
                databaseOperations[DatabaseOperations.SubTotal] = new Version(9, 5);

                return databaseOperations;
            }
        }
    }
}
