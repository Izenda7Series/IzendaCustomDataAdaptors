using Izenda.BI.Framework;
using System;
using System.Collections.Generic;
using Izenda.BI.Framework.Models;
using System.Data;
using Izenda.BI.Framework.Constants;
using Npgsql;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift
{
    public class RedshiftConnection : IConnection
    {
        public string GetDatabaseName(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            return builder.Database;
        }

        public string GetDatabaseServer(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            return builder.Host;
        }

        public string GetUserName(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            return builder.UserName;
        }

        public IDbConnection OpenConnection(string connectionString)
        {
            var connection = new NpgsqlConnection(connectionString);
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
                var databaseName = GetDatabaseName(connectionString);

                if (string.IsNullOrEmpty(databaseName))
                {
                    result.Status = ConnectDBStatus.DatabaseNotValid;
                    return result;
                }

                using (var connection = OpenConnection(connectionString))
                {
                    result.Status = ConnectDBStatus.Success;
                    return result;
                }
            }
            catch (NpgsqlException ex)
            {
                var invalidServerMessage = new List<string> { "08000", "08003", "08006", "08001", "08004", "08007", "08P01" }; //These numbers is status code return by database relate to server name or network issue.
                var invalidDatabase = "3D000"; //Error code return when database is not valid
                var loginFail = new List<string> { "28000", "28P01" }; //Error code return when cannot loggin database

                if (invalidServerMessage.Contains(ex.Code))
                {
                    result.Status = ConnectDBStatus.ServerNotValid;
                }
                else if (ex.Code == invalidDatabase)
                {
                    result.Status = ConnectDBStatus.DatabaseNotValid;
                }
                else if (loginFail.Contains(ex.Code))
                {
                    result.Status = ConnectDBStatus.LoginFail;
                }
                else
                {
                    result.Status = ConnectDBStatus.Fail;
                }
            }
            catch
            {
                result.Status = ConnectDBStatus.Fail;
            }

            return result;
        }
    }
}
