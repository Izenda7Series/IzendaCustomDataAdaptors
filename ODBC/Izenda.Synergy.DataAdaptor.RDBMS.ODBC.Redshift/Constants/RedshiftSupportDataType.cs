// ---------------------------------------------------------------------- 
// <copyright file="RedshiftSupportDataType.cs" company="Izenda">
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
using Izenda.BI.Framework.Constants;
using Izenda.BI.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBC.Redshift.Constants
{
    /// <summary>
    /// The oracle data type model
    /// </summary>    
    public class RedshiftSupportDataType : DatabaseSupportDataType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public void AddRedshiftSupportDataType()
        {
            // DateTime
            AddDatabaseDataType("DATE", IzendaDataType.DatetimeType, true, "System.DateTime");
            AddDatabaseDataType("TIMESTAMP WITHOUT TIME ZONE", IzendaDataType.DatetimeType, true, "System.DateTime");
            AddDatabaseDataType("TIMESTAMP WITH TIME ZONE", IzendaDataType.DatetimeType, true, "System.DateTime");

            // Numeric
            AddDatabaseDataType("SMALLINT", IzendaDataType.NumericType, true, "System.Int16");
            AddDatabaseDataType("INT2", IzendaDataType.NumericType, true, "System.Int16");
            AddDatabaseDataType("INTEGER", IzendaDataType.NumericType, true, "System.Int32");
            AddDatabaseDataType("INT", IzendaDataType.NumericType, true, "System.Int32");
            AddDatabaseDataType("INT4", IzendaDataType.NumericType, true, "System.Int32");
            AddDatabaseDataType("BIGINT", IzendaDataType.NumericType, true, "System.Int64");
            AddDatabaseDataType("INT8", IzendaDataType.NumericType, true, "System.Int64");
            AddDatabaseDataType("REAL", IzendaDataType.NumericType, true, "System.Single");
            AddDatabaseDataType("FLOAT4", IzendaDataType.NumericType, true, "System.Single");
            AddDatabaseDataType("DOUBLE PRECISION", IzendaDataType.NumericType, true, "System.Double");
            AddDatabaseDataType("FLOAT", IzendaDataType.NumericType, true, "System.Double");
            AddDatabaseDataType("FLOAT8", IzendaDataType.NumericType, true, "System.Double");
            AddDatabaseDataType("DECIMAL", IzendaDataType.NumericType, true, "System.Decimal");
            AddDatabaseDataType("NUMERIC", IzendaDataType.NumericType, true, "System.Decimal");

            // Boolean
            AddDatabaseDataType("BOOLEAN", IzendaDataType.BooleanType, true, "System.Boolean", true /* default mapping */);
            AddDatabaseDataType("BOOL", IzendaDataType.BooleanType, true, "System.Boolean" /* default mapping */);

            //// Text
            AddDatabaseDataType("CHAR", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("CHARACTER", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("NCHAR", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("BPCHAR", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("VARCHAR", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("CHARACTER VARYING", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("NVARCHAR", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("TEXT", IzendaDataType.TextType, true, "System.String");
        }
    }
}
