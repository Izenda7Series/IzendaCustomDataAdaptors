// ---------------------------------------------------------------------- 
// <copyright file="OracleDatabaseDataType.cs" company="Izenda">
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

namespace Izenda.BI.DataAdaptor.RDBMS.DB2.Constants
{
    /// <summary>
    /// The oracle data type model
    /// </summary>    
    public class DB2SupportDataType : DatabaseSupportDataType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DB2SupportDataType()
        {
            // Numeric
            AddDatabaseDataType("smallint", IzendaDataType.NumericType, true, "System.Int16");
            AddDatabaseDataType("int", IzendaDataType.NumericType, true, "System.Int32");
            AddDatabaseDataType("bigint", IzendaDataType.NumericType, true, "System.Int64");
            AddDatabaseDataType("real", IzendaDataType.NumericType, true, "System.Single");
            AddDatabaseDataType("double precision", IzendaDataType.NumericType, true, "System.Double");
            AddDatabaseDataType("float", IzendaDataType.NumericType, true, "System.Double");
            AddDatabaseDataType("decimal", IzendaDataType.NumericType, true, "System.Decimal");
            AddDatabaseDataType("decfloat", IzendaDataType.NumericType, true, "System.Decimal");

            // DateTime
            AddDatabaseDataType("date", IzendaDataType.DatetimeType, true, "System.DateTime");
            AddDatabaseDataType("time", IzendaDataType.DatetimeType, true, "System.DateTime");
            AddDatabaseDataType("timestamp", IzendaDataType.DatetimeType, true, "System.DateTime");

            // Xml
            AddDatabaseDataType("xml", IzendaDataType.XMLType, false, "System.Byte[]");

            // Text
            AddDatabaseDataType("char", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("nvarchar", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("long varchar", IzendaDataType.TextType, true, "System.String");

            // Binary data
            AddDatabaseDataType("char for bit data", IzendaDataType.LobType, false, "System.Byte[]");
            AddDatabaseDataType("binary", IzendaDataType.LobType, false, "System.Byte[]");
            AddDatabaseDataType("varbinary", IzendaDataType.LobType, false, "System.Byte[]");
            AddDatabaseDataType("long varchar for bit data", IzendaDataType.LobType, false, "System.Byte[]");

            // Graphic data
            AddDatabaseDataType("graphic", IzendaDataType.TextType, false, "System.String");
            AddDatabaseDataType("vargraphic", IzendaDataType.TextType, false, "System.String");
            AddDatabaseDataType("long vargraphic", IzendaDataType.TextType, false, "System.String");

            // Lob
            AddDatabaseDataType("clob", IzendaDataType.LobType, false, "System.String");
            AddDatabaseDataType("blob", IzendaDataType.LobType, false, "System.Byte[]");
            AddDatabaseDataType("dbclob", IzendaDataType.LobType, false, "System.String");

            // Row ID
            AddDatabaseDataType("rowid", IzendaDataType.LobType, false, "System.Byte[]");
        }
    }
}
