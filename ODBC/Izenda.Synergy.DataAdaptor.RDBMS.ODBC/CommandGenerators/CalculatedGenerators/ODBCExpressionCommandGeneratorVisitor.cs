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
using Izenda.BI.DataAdaptor.RDBMS.MyODBC.CommandGenerators;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBC.CommandGenerators
{
    /// <summary>
    /// ODBCExpressionCommandGeneratorVisitor
    /// </summary>
    /// <seealso cref="Izenda.BI.DataAdaptor.RDBMS.CommandGenerators.ExpressionCommandGeneratorVisitor" />
    public class ODBCExpressionCommandGeneratorVisitor : ExpressionCommandGeneratorVisitor
    {
        /// <summary>
        /// Gets the cast token command generator.
        /// </summary>
        public override AvgTokenCommandGenerator AvgTokenCommandGenerator
        {
            get
            {
                return new ODBCAvgTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the cast token command generator.
        /// </summary>
        public override SumTokenCommandGenerator SumTokenCommandGenerator
        {
            get
            {
                return new ODBCSumTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the convert token command generator.
        /// </summary>
        public override ConvertTokenCommandGenerator ConvertTokenCommandGenerator
        {
            get
            {
                return new ODBCConvertTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the cast token command generator.
        /// </summary>
        public override CastTokenCommandGenerator CastTokenCommandGenerator
        {
            get
            {
                return new ODBCCastTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the date difference token command generator.
        /// </summary>        
        public override DateDiffTokenCommandGenerator DateDiffTokenCommandGenerator
        {
            get
            {
                return new ODBCDateDiffTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the date part token command generator.
        /// </summary>        
        public override DatePartTokenCommandGenerator DatePartTokenCommandGenerator
        {
            get
            {
                return new ODBCDatePartTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the date add token command generator.
        /// </summary>
        public override DateAddTokenCommandGenerator DateAddTokenCommandGenerator
        {
            get
            {
                return new ODBCDateAddTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the get date token command generator.
        /// </summary>        
        public override GetDateTokenCommandGenerator GetDateTokenCommandGenerator
        {
            get
            {
                return new ODBCGetDateTokenCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the length token command generator.
        /// </summary>       
        public override LenTokenCommandGenerator LenTokenCommandGenerator
        {
            get
            {
                return new ODBCLenTokenCommandGenerator(this);
            }
        }

        public override ValueTokenCommandGenerator ValueTokenCommandGenerator
        {
            get
            {
                var valueTokenCommandGenerator = new ODBCValueTokenCommandGenerator(this)
                {
                    Parameters = this.Parameters
                };
                return valueTokenCommandGenerator;
            }
        }

        /// <summary>
        /// Gets the length token command generator.
        /// </summary>
        public override LpadTokenCommandGenerator LpadTokenCommandGenerator
        {
            get
            {
                return new ODBCLpadTokenCommandGenerator(this);
            }
        }

    }
}
