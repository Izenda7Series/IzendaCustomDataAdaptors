// ---------------------------------------------------------------------- 
// <copyright file="ODBCLpadTokenCommandGenerator.cs" company="Izenda">
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

using Izenda.BI.Framework.Components.ExpressionEvaluations;
using Izenda.BI.Framework.Components.ExpressionEvaluations.Functions;
using System;

namespace Izenda.BI.DataAdaptor.RDBMS.CommandGenerators
{
    /// <summary>
    /// LpadTokenCommandGenerator
    /// </summary>
    /// <seealso cref="Izenda.BI.DataAdaptor.RDBMS.CommandGenerators.DatabaseFunctionTokenCommandGenerator" />
    public class ODBCLpadTokenCommandGenerator: LpadTokenCommandGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODBCLpadTokenCommandGenerator"/> class.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public ODBCLpadTokenCommandGenerator(ExpressionCommandGeneratorVisitor visitor) : base(visitor)
        {

        }

        /// <summary>
        /// Generate query for Token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns>
        /// The query
        /// </returns>
        public override string GenerateSelfCommand(Token token)
        {
            var functionToken = token as DatabaseFunctionToken;
            var subTrees = functionToken.SubTrees;

            string expression = visitor.NodeData[(subTrees["1"] as Token).TokenId];
            var length = visitor.NodeData[(subTrees["2"] as Token).TokenId];
            var paddingCharactor = visitor.NodeData[(subTrees["3"] as Token).TokenId];
            var lengthValue = Convert.ToInt32(length);


            return string.Format("LPAD({0},{1},{2})", expression, length, paddingCharactor);
        }
    }
}
