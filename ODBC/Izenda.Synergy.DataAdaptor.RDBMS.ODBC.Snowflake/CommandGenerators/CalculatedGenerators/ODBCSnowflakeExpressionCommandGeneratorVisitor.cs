// ---------------------------------------------------------------------- 
// <copyright file="ODBCSnowflakeExpressionCommandGeneratorVisitor.cs" company="Izenda">
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
using Izenda.BI.DataAdaptor.RDBMS.ODBC.CommandGenerators;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBCSnowflake.CommandGenerators
{
    /// <summary>
    /// ODBCSnowflakeExpressionCommandGeneratorVisitor
    /// </summary>
    /// <seealso cref="Izenda.BI.DataAdaptor.RDBMS.CommandGenerators.ExpressionCommandGeneratorVisitor" />
    public class ODBCSnowflakeExpressionCommandGeneratorVisitor : ODBCExpressionCommandGeneratorVisitor
    {
        /// <summary>
        /// Gets the convert token command generator.
        /// </summary>
        public override ConvertTokenCommandGenerator ConvertTokenCommandGenerator
        {
            get
            {
                return new ODBCSnowflakeConvertTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the cast token command generator.
        /// </summary>
        public override CastTokenCommandGenerator CastTokenCommandGenerator
        {
            get
            {
                return new ODBCSnowflakeCastTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the date difference token command generator.
        /// </summary>        
        public override DateDiffTokenCommandGenerator DateDiffTokenCommandGenerator
        {
            get
            {
                return new ODBCSnowflakeDateDiffTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the date part token command generator.
        /// </summary>        
        public override DatePartTokenCommandGenerator DatePartTokenCommandGenerator
        {
            get
            {
                return new ODBCSnowflakeDatePartTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the date add token command generator.
        /// </summary>
        public override DateAddTokenCommandGenerator DateAddTokenCommandGenerator
        {
            get
            {
                return new ODBCSnowflakeDateAddTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the get date token command generator.
        /// </summary>        
        public override GetDateTokenCommandGenerator GetDateTokenCommandGenerator
        {
            get
            {
                return new ODBCSnowflakeGetDateTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the length token command generator.
        /// </summary>       
        public override LenTokenCommandGenerator LenTokenCommandGenerator
        {
            get
            {
                return new ODBCSnowflakeLenTokenCommandGenerator(this);
            }
        }
    }
}
