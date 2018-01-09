// ---------------------------------------------------------------------- 
// <copyright file="ODBCRedshiftQueryTreeCommandGenerator.cs" company="Izenda">
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
//  CONSULT THE END USER LICENSE AGREEMENT(EULA FOR INFORMATION ON  
//  ADDITIONAL RESTRICTIONS.
// </copyright> 
// ----------------------------------------------------------------------

using Izenda.BI.DataAdaptor.RDBMS.ODBC.CommandGenerators;
using Izenda.BI.Framework.Components.QueryExpressionTree;
using Izenda.BI.Framework.Models;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBCRedshift.CommandGenerators
{
    public class ODBCRedshiftQueryTreeCommandGenerator : ODBCQueryTreeCommandGenerator
    {
        /// <summary>
        /// Generate query for operand
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="context"></param>
        /// <returns>
        /// The query
        /// </returns>
        protected override string GenerateCommand(QueryTreeNode operand, FusionContextData context)
        {
            var visitor = new ODBCRedshiftQueryTreeCommandGeneratorVisitor();
            visitor.ContextData = context;
            operand.Accept(visitor);

            return visitor.NodeData[operand.Id];
        }

        /// <summary>
        /// Applies the advanced setting.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void ApplyAdvancedSetting(FusionContextData context)
        {

        }
    }
}
