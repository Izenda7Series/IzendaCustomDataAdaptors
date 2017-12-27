// ---------------------------------------------------------------------- 
// <copyright file="OracleConnection.cs" company="Izenda">
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
//  CONSULT THE END USER LICENSE AGREEMENT(EULA) FOR INFORMATION ON  
//  ADDITIONAL RESTRICTIONS.
// </copyright> 
// ----------------------------------------------------------------------

using Izenda.BI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Izenda.BI.Framework.Models;
using System.Data;
using Izenda.BI.Framework.Constants;
using System.Data.Odbc;

namespace Izenda.Synergy.DataAdaptor.RDBMS.ODBC
{
    public class ODBCConnection : IConnection
    {
        public string GetDatabaseName(string connectionString)
        {
            var builder = new OdbcConnectionStringBuilder(connectionString);
            return builder["database"].ToString();
        }

        public string GetDatabaseServer(string connectionString)
        {
            var builder = new OdbcConnectionStringBuilder(connectionString);
            return builder["server"].ToString();
        }

        public string GetUserName(string connectionString)
        {
            var builder = new OdbcConnectionStringBuilder(connectionString);
            return builder["uid"].ToString();
        }

        public virtual IDbConnection OpenConnection(string connectionString)
        {
            var connection = new OdbcConnection(connectionString);
            connection.Open();
            return connection;
        }

        public virtual ConnectionStatus TestConnection(Guid serverType, string connectionString)
        {
            var result = new ConnectionStatus
            {
                Status = ConnectDBStatus.Success,
                ConnectionString = connectionString
            };

            try
            {
                using (var connection = OpenConnection(connectionString))
                {
                    result.Status = ConnectDBStatus.Success;
                    return result;
                }
            }
            catch (OdbcException ex)
            {
                result.Status = ConnectDBStatus.Fail;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}
