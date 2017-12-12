// ---------------------------------------------------------------------- 
// <copyright file="SnowflakeSupportDataType.cs" company="Izenda">
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

namespace Izenda.BI.DataAdaptor.RDBMS.ODBC.Constants
{
    /// <summary>
    /// The oracle data type model
    /// </summary>    
    public class SnowflakeSupportDataType : DatabaseSupportDataType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SnowflakeSupportDataType()
        {
            //// DateTime
            AddDatabaseDataType("DATE", IzendaDataType.DatetimeType, true, "System.DateTime");
            AddDatabaseDataType("DATETIME", IzendaDataType.DatetimeType, true, "System.DateTime");
            AddDatabaseDataType("TIME", IzendaDataType.DatetimeType, true, "System.DateTime");
            AddDatabaseDataType("TIMESTAMP", IzendaDataType.DatetimeType, true, "System.DateTime");
            AddDatabaseDataType("TIMESTAMP_LTZ", IzendaDataType.DatetimeType, true, "System.DateTime");
            AddDatabaseDataType("TIMESTAMP_NTZ", IzendaDataType.DatetimeType, true, "System.DateTime");
            AddDatabaseDataType("TIMESTAMP_TZ", IzendaDataType.DatetimeType, true, "System.DateTime");

            //// Numeric
            AddDatabaseDataType("NUMBER", IzendaDataType.NumericType, true, "System.Decimal");
            AddDatabaseDataType("DECIMAL", IzendaDataType.NumericType, true, "System.Decimal");
            AddDatabaseDataType("NUMERIC", IzendaDataType.NumericType, true, "System.Decimal");
            AddDatabaseDataType("INT", IzendaDataType.NumericType, true, "System.Int32");
            AddDatabaseDataType("INTEGER", IzendaDataType.NumericType, true, "System.Int32");
            AddDatabaseDataType("BIGINT", IzendaDataType.NumericType, true, "System.Int64");
            AddDatabaseDataType("SMALLINT", IzendaDataType.NumericType, true, "System.Int16");
            AddDatabaseDataType("TINYINT", IzendaDataType.NumericType, true, "System.Int32");
            AddDatabaseDataType("BYTEINT", IzendaDataType.NumericType, true, "System.Byte");
            AddDatabaseDataType("FLOAT", IzendaDataType.NumericType, true, "System.Double");
            AddDatabaseDataType("FLOAT4", IzendaDataType.NumericType, true, "System.Single");
            AddDatabaseDataType("FLOAT8", IzendaDataType.NumericType, true, "System.Double");
            AddDatabaseDataType("DOUBLE", IzendaDataType.NumericType, true, "System.Double");
            AddDatabaseDataType("DOUBLE PRECISION", IzendaDataType.NumericType, true, "System.Double");
            AddDatabaseDataType("REAL", IzendaDataType.NumericType, true, "System.Single");

            // Boolean
            AddDatabaseDataType("BOOLEAN", IzendaDataType.BooleanType, true, "System.Boolean", true /* default mapping */);
            AddDatabaseDataType("BOOL", IzendaDataType.BooleanType, true, "System.Boolean" /* default mapping */);

            /// Text
            AddDatabaseDataType("VARCHAR", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("CHAR", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("CHARACTER", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("STRING", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("TEXT", IzendaDataType.TextType, true, "System.String");

            // Binary
            AddDatabaseDataType("BINARY", IzendaDataType.LobType, true, "System.Byte[]");
            AddDatabaseDataType("VARBINARY", IzendaDataType.LobType, true, "System.Byte[]");

            // Semi-structured Data Types
            AddDatabaseDataType("VARIANT", IzendaDataType.LobType, true, "System.Object");
            AddDatabaseDataType("OBJECT", IzendaDataType.LobType, true, "System.Object");
            AddDatabaseDataType("ARRAY", IzendaDataType.TextType, true, "System.Object");
        }
    }
}
