// ---------------------------------------------------------------------- 
// <copyright file="ODBCSnowflakeSelectFieldCommandGenerator.cs" company="Izenda">
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

namespace Izenda.BI.DataAdaptor.RDBMS.ODBCSnowflake.CommandGenerators
{
    /// <summary>
    /// Generate command for operand
    /// </summary>
    public class ODBCSnowflakeSelectFieldCommandGenerator : ODBCSelectFieldCommandGenerator
    {
        /// <summary>
        /// Contructor
        /// </summary>
        public ODBCSnowflakeSelectFieldCommandGenerator(QueryTreeCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        /// <summary>
        /// Gets the convert null to empty namespace.
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        protected override string SpecificNamespaceForConvertNullToEmpty
        {
            get
            {
                return string.Format(ConvertNullToEmptyNamespace, ".ODBCSnowflake", ".ODBCSnowflake", this.GetType().Assembly.GetName().Name);
            }
        }
    }
}
