// ---------------------------------------------------------------------- 
// <copyright file="ODBCExpressionCommandGeneratorVisitor.cs" company="Izenda">
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

namespace Izenda.BI.DataAdaptor.RDBMS.ODBCRedshift.CommandGenerators
{
    /// <summary>
    /// ODBCExpressionCommandGeneratorVisitor
    /// </summary>
    /// <seealso cref="Izenda.BI.DataAdaptor.RDBMS.CommandGenerators.ExpressionCommandGeneratorVisitor" />
    public class ODBCRedshiftExpressionCommandGeneratorVisitor : ODBCExpressionCommandGeneratorVisitor
    {
        /// <summary>
        /// Gets the convert token command generator.
        /// </summary>
        public override ConvertTokenCommandGenerator ConvertTokenCommandGenerator
        {
            get
            {
                return new ODBCRedshiftConvertTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the cast token command generator.
        /// </summary>
        public override CastTokenCommandGenerator CastTokenCommandGenerator
        {
            get
            {
                return new ODBCRedshiftCastTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the date difference token command generator.
        /// </summary>        
        public override DateDiffTokenCommandGenerator DateDiffTokenCommandGenerator
        {
            get
            {
                return new ODBCRedshiftDateDiffTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the date part token command generator.
        /// </summary>        
        public override DatePartTokenCommandGenerator DatePartTokenCommandGenerator
        {
            get
            {
                return new ODBCRedshiftDatePartTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the date add token command generator.
        /// </summary>
        public override DateAddTokenCommandGenerator DateAddTokenCommandGenerator
        {
            get
            {
                return new ODBCRedshiftDateAddTokenCommandGenerator(this);
            }
        }
        
        /// <summary>
        /// Gets the is null token command generator.
        /// </summary>        
        public override IsNullTokenCommandGenerator IsNullTokenCommandGenerator
        {
            get
            {
                return new ODBCRedshiftIsNullTokenCommandGenerator(this);
            }
        }
    }
}
