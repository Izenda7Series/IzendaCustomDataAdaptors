// ---------------------------------------------------------------------- 
// <copyright file="DB2ReplaceDateTruncateFunctionActivity.cs" company="Izenda">
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


using Izenda.BI.Framework.Models.Contexts;
using System;

namespace Izenda.BI.QueryNormalizer.DB2
{
    /// <summary>
    /// Replace date truncate function
    /// </summary>
    public class DB2ReplaceDateTruncateFunctionActivity : DB2QueryNormalizerActivity
    {
        /// <summary>
        /// The activity order
        /// </summary>
        public override int Order
        {
            get
            {
                return 12;
            }
        }

        /// <summary>
        /// Execute the activity
        /// </summary>
        /// <param name="context">The context</param>
        public override void Execute(QueryNormalizerContext context)
        {
            var DB2 = context.Query;
            int index = -1;
            const string dateTruncate = "DATETRUNCATE";

            index = DB2.IndexOf(dateTruncate, StringComparison.OrdinalIgnoreCase);

            while (index >= 0)
            {
                var openIndex = DB2.IndexOf("(", index);
                var closeIndex = DB2.IndexOf(")", index);
                var fieldName = DB2.Substring(openIndex + 1, closeIndex - 1 - openIndex);
                var replacedContent = fieldName;
                var originalContent = DB2.Substring(index, closeIndex - index + 1);

                DB2 = DB2.Replace(originalContent, replacedContent);

                index = DB2.IndexOf(dateTruncate, StringComparison.OrdinalIgnoreCase);
            }

            context.Query = DB2;
        }
    }
}
