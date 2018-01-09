// ---------------------------------------------------------------------- 
// <copyright file="ODBCSnowflakeLenTokenCommandGenerator.cs" company="Izenda">
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
using Izenda.BI.DataAdaptor.RDBMS.Constants;
using Izenda.BI.DataAdaptor.RDBMS.ODBC.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.ODBCSnowflake.Constants;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBCSnowflake.CommandGenerators
{
    /// <summary>
    /// ODBCSnowflakeLenTokenCommandGenerator
    /// </summary>
    /// <seealso cref="Izenda.BI.DataAdaptor.RDBMS.CommandGenerators.DateAddTokenCommandGenerator" />
    public class ODBCSnowflakeLenTokenCommandGenerator : ODBCLenTokenCommandGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODBCSnowflakeLenTokenCommandGenerator"/> class.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public ODBCSnowflakeLenTokenCommandGenerator(ExpressionCommandGeneratorVisitor visitor) : base(visitor)
        {

        }

        /// <summary>
        /// Gets the database function.
        /// </summary>        
        public override DatabaseFunction DatabaseFunction
        {
            get
            {
                return new SnowflakeDatabaseFunction();
            }
        }
    }
}
