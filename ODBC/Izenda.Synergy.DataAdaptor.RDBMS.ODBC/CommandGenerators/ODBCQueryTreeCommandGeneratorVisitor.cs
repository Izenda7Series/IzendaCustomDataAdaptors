﻿// ---------------------------------------------------------------------- 
// <copyright file="ODBCQueryTreeCommandGeneratorVisitor.cs" company="Izenda">
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

namespace Izenda.BI.DataAdaptor.RDBMS.ODBC.CommandGenerators
{
    /// <summary>
    /// ODBCQueryTreeCommandGeneratorVisitor
    /// </summary>
    /// <seealso cref="Izenda.BI.DataAdaptor.RDBMS.CommandGenerators.QueryTreeCommandGeneratorVisitor" />
    public class ODBCQueryTreeCommandGeneratorVisitor : QueryTreeCommandGeneratorVisitor
    {
        /// <summary>
        /// Gets the paging operator command generator.
        /// </summary>
        public override PagingOperatorCommandGenerator PagingOperatorCommandGenerator
        {
            get
            {
                return new ODBCPagingOperatorCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the operand command generator.
        /// </summary>
        public override OperandCommandGenerator OperandCommandGenerator
        {
            get
            {
                return new ODBCOperandCommandGenerator(this);
            }
        }

        /// <summary>
        /// Get the join operator command generator
        /// </summary>
        public override JoinOperatorCommandGenerator JoinOperatorCommandGenerator
        {
            get
            {
                return new ODBCJoinOperatorCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the projection command generator.
        /// </summary>
        public override ProjectionOperatorCommandGenerator ProjectionOperatorCommandGenerator
        {
            get
            {
                return new ODBCProjectionOperatorCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the selection command generator.
        /// </summary>
        public override SelectionOperatorCommandGenerator SelectionOperatorCommandGenerator
        {
            get
            {
                return new ODBCSelectionOperatorCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the grouping command generator.
        /// </summary>
        public override GroupingOperatorCommandGenerator GroupingOperatorCommandGenerator
        {
            get
            {
                return new ODBCGroupingOperatorCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the result limit command generator.
        /// </summary>
        public override ResultLimitOperatorCommandGenerator ResultLimitOperatorCommandGenerator
        {
            get
            {
                return new ODBCResultLimitOperatorCommandGenerator(this);
            }
        }

        /// <summary>
        /// Gets the convert null to empty command generator.
        /// </summary>
        public override ConvertNullToEmptyOperatorCommandGenerator ConvertNullToEmptyOperatorCommandGenerator
        {
            get
            {
                return new ODBCConvertNullToEmptyOperatorCommandGenerator(this);
            }
        }
    }
}
