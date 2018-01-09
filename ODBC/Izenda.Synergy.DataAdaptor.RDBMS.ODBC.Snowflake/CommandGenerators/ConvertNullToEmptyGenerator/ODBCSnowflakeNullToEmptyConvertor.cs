// ---------------------------------------------------------------------- 
// <copyright file="ODBCSnowflakeNullToEmptyConvertor.cs" company="Izenda">
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

using System.Text;
using Izenda.BI.Framework.Models;
using Izenda.BI.DataAdaptor.RDBMS.ODBC.CommandGenerators.ConvertNullToEmptyGenerator;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBCSnowflake.CommandGenerators.ConvertNullToEmptyGenerator
{
    /// <summary>
    /// ODBCSnowflakeNullToEmptyConvertor
    /// </summary>    
    public class ODBCSnowflakeNullToEmptyConvertor : ODBCNullToEmptyConvertor
    {
        /// <summary>
        /// Generates the select command.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="fieldAlias">The field alias.</param>
        /// <param name="querySourceField">The query source field.</param>
        /// <returns></returns>
        public override StringBuilder GenerateSelectCommand(StringBuilder stringBuilder, string fieldAlias, QuerySourceField querySourceField = null)
        {
            if (querySourceField != null)
            {
                return stringBuilder.AppendFormat(@"NVL([[{0}]],'') AS ""{1}"",", querySourceField.Name, fieldAlias);
            }

            return stringBuilder.AppendFormat(@"NVL([[{0}]],'') AS ""{1}"",", fieldAlias, fieldAlias);
        }

        /// <summary>
        /// Generates the select command format.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="fieldAlias">The field alias.</param>
        /// <param name="querySourceField">The query source field.</param>
        /// <returns></returns>
        public override StringBuilder GenerateSelectCommandFormat(StringBuilder stringBuilder, string fieldAlias, QuerySourceField querySourceField = null)
        {
            if (querySourceField != null)
            {
                return stringBuilder.AppendFormat(@"NVL({0},'') AS ""{1}"",", querySourceField.Name, fieldAlias);
            }

            return stringBuilder.AppendFormat(@"NVL({0},'') AS ""{1}"",", fieldAlias, fieldAlias);
        }
    }
}
