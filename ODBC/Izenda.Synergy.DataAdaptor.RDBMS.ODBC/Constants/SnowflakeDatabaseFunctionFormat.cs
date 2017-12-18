// ---------------------------------------------------------------------- 
// <copyright file="SnowflakeDatabaseFunctionFormat.cs" company="Izenda">
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


using Izenda.BI.DataAdaptor.RDBMS.Constants;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBC.Constants
{
    /// <summary>
    /// Database Function Format
    /// </summary>
    public class SnowflakeDatabaseFunctionFormat : DatabaseFunctionFormat
    {
        /// <summary>
        /// Get date diff month format
        /// </summary>
        /// <returns>The date diff month format</returns>
        public override string DateDiffMonthFormat
        {
            get
            {
                return "DATEDIFF(month, {0}::timestamp, {1}::timestamp)";
            }
        }

        /// <summary>
        /// Get date diff year format
        /// </summary>
        /// <returns>The date diff year format</returns>
        public override string DateDiffYearFormat
        {
            get
            {
                return "DATEDIFF(year, {0}::timestamp, {1}::timestamp)";
            }
        }

        /// <summary>
        /// Get date diff day format
        /// </summary>
        /// <returns>The date diff year format</returns>
        public override string DateDiffDayFormat
        {
            get
            {
                return "DATEDIFF(day, {0}::timestamp, {1}::timestamp)";
            }
        }

        /// <summary>
        /// Gets the check blank format text.
        /// </summary>       
        public override string CheckBlankFormatText
        {
            get
            {
                return @"[[{0}]] = ''";
            }
        }

        /// <summary>
        /// Check not blank format
        /// </summary>
        public override string CheckNotBlankFormat
        {
            get
            {
                return "([[{0}]] IS NULL OR [[{0}]] <> '')";
            }
        }

        /// <summary>
        /// Gets the check not blank format text.
        /// </summary>     
        public override string CheckNotBlankFormatText
        {
            get
            {
                return "([[{0}]] IS NULL OR [[{0}]] <> '')";
            }
        }
    }
}
