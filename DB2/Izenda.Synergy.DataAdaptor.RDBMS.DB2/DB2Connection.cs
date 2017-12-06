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
using IBM.Data.DB2;

namespace Izenda.Synergy.DataAdaptor.RDBMS.DB2
{
    public class IBMDB2Connection : IConnection
    {
        public string GetDatabaseName(string connectionString)
        {
            var builder = new DB2ConnectionStringBuilder(connectionString);
            return builder.Database;
        }

        public string GetDatabaseServer(string connectionString)
        {
            var builder = new DB2ConnectionStringBuilder(connectionString);
            return builder.Server;
        }

        public string GetUserName(string connectionString)
        {
            var builder = new DB2ConnectionStringBuilder(connectionString);
            return builder.UserID;
        }

        public IDbConnection OpenConnection(string connectionString)
        {
            var connection = new IBM.Data.DB2.DB2Connection(connectionString);
            connection.Open();
            return connection;
        }

        public ConnectionStatus TestConnection(Guid serverType, string connectionString)
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
            catch (DB2Exception ex)
            {
                if (CheckDB2ErrorMessage(ex, "SQL1336N"))
                {
                    result.Status = ConnectDBStatus.ServerNotValid;
                }
                else if (CheckDB2ErrorMessage(ex, "SQL30061N"))
                {
                    result.Status = ConnectDBStatus.DatabaseNotValid;
                }
                else if (CheckDB2ErrorMessage(ex, "SQL30082N"))
                {
                    result.Status = ConnectDBStatus.LoginFail;
                }
                else
                {
                    result.Status = ConnectDBStatus.Fail;
                }

                result.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                result.Status = ConnectDBStatus.Fail;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        private bool CheckDB2ErrorMessage(DB2Exception ex, string SQLMessageCode)
        {
            for (int i = 0; i < ex.Errors.Count; i++)
            {
                var error = ex.Errors[i];
                if (error.Message.Contains(SQLMessageCode))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
