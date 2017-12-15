// ---------------------------------------------------------------------- 
// <copyright file="ODBCSumTokenCommandGenerator.cs" company="Izenda">
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

using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.Framework.Components.ExpressionEvaluations;
using Izenda.BI.Framework.Components.ExpressionEvaluations.Functions;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBC.CommandGenerators
{
    /// <summary>
    /// SumTokenCommandGenerator
    /// </summary>
    /// <seealso cref="Izenda.BI.DataAdaptor.RDBMS.CommandGenerators.AggregateFunctionTokenCommandGenerator" />
    public class ODBCSumTokenCommandGenerator : SumTokenCommandGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SumTokenCommandGenerator"/> class.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public ODBCSumTokenCommandGenerator(ExpressionCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        /// <summary>
        /// Generate query for Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns>
        /// The query
        /// </returns>
        public override string GenerateSelfCommand(Token token)
        {
            var aggregatedFunctionToken = token as AggregateToken;
            var subTrees = aggregatedFunctionToken.SubTrees;

            string expression = visitor.NodeData[(subTrees["1"] as Token).TokenId];

            if (!(subTrees["1"] is DistinctToken))
            {
                return string.Format("{0}({2} CAST({1} AS DECIMAL(28,8)))",
                                     "SUM",
                                     expression,
                                     aggregatedFunctionToken.Distinct ? DatabaseFunction.Distinct + " " : ""
                                     );
            }
            else
            {
                //#16022: ODBC does not work for sum(cast(distint ...)))
                return string.Format("{0}({1})",
                                     "SUM",
                                     expression
                                     );
            }
        }
    }
}
