using Dapper;
using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.Redshift.Constants;
using Izenda.BI.DataAdaptor.SQL.SchemaLoader;
using Izenda.BI.Framework;
using Izenda.BI.Framework.Components.SequenceWorkflows;
using Izenda.BI.Framework.Constants;
using Izenda.BI.Framework.Exceptions;
using Izenda.BI.Framework.Models;
using Izenda.BI.Framework.Models.Contexts;
using Izenda.BI.QueryNormalizer.Redshift;
using Izenda.BI.Resource;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift
{
    [Export(typeof(IDataSourceAdaptor))]
    [ExportMetadata("ServerType", "E285BFD1-F8D5-4BEB-A345-B3D2EF5A3DE8" + "|AWSRS|[AWSRS] Redshift")]
    public class RedshiftDataSourceAdaptor : DataSourceAdaptor
    {
        ISequenceWorkflow<RedshiftQueryNormalizerActivity, QueryNormalizerContext> queryNormalizer;

        protected ISequenceWorkflow<RedshiftQueryNormalizerActivity, QueryNormalizerContext> QueryNormalizer
        {
            get
            {
                if (queryNormalizer == null)
                {
                    queryNormalizer = new SequenceWorkflow<RedshiftQueryNormalizerActivity, QueryNormalizerContext>();
                }

                return queryNormalizer;
            }
        }

        public override ISchemaLoader SchemaLoader => new RedshiftSchemaLoader();

        public override IConnection Connection => new RedshiftConnection();

        public override QueryTreeCommandGenerator QueryTreeCommandGenerator
        {
            get
            {
                return new RedshiftQueryTreeCommandGenerator();
            }
        }

        public override string GetFirstValueInFilteredQuery
        {
            get
            {
                return @"SELECT ""{0}"" FROM {1} LIMIT 1";
            }
        }

        public override string GetConnectionStringWithServerAndDatabaseName(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);

            if (builder.IntegratedSecurity)
            {
                return connectionString;
            }

            return string.Format("Server={0}; Database={1};", builder.Host, builder.Database);
        }

        protected override string NormalizeQuery(string query)
        {
            var normalizerContext = new QueryNormalizerContext { Query = query };
            QueryNormalizer.Execute(normalizerContext);
            return normalizerContext.Query;
        }

        public override List<DatabaseDataType> GetBaseDataTypes()
        {
            var dataTypeAdaptor = new RedshiftSupportDataType();
            return dataTypeAdaptor.GetBaseDataTypes();
        }

        public override IEnumerable<T> Query<T>(string connectionString, string query, object param = null, int queryTimeout = 60)
        {
            var standardQuery = NormalizeQuery(query);

            using (var connection = OpenConnection(connectionString))
            {
                try
                {
                    return connection.Query<T>(standardQuery, param, commandTimeout: queryTimeout);
                }
                catch (NpgsqlException ex)
                {
                    Log($"QUERY: {standardQuery}", LogType.Error);
                    Log("Query error: " + ex.ToString(), LogType.Error);

                    var incorrectSyntax = new List<string> { "42000", "42601" };

                    if (incorrectSyntax.Contains(ex.Code))
                    {
                        //Hide detail incorect syntax message. This may contain sensitive information
                        throw new IzendaException(Messages.FusionQueryInCorrectSyntax);
                    }

                    throw new FusionException(Messages.FusionCanNotQueryData);
                }
            }
        }

        public override IEnumerable<T> QueryMultiple<T>(string connectionString, string query, object param = null, int queryTimeout = 60, Action<SqlMapper.GridReader> action = null)
        {
            var normalizerContext = new QueryNormalizerContext { Query = query };
            QueryNormalizer.Execute(normalizerContext);
            query = normalizerContext.Query;
            using (var connection = OpenConnection(connectionString))
            {
                try
                {
                    using (var data = connection.QueryMultiple(query, param, commandTimeout: queryTimeout))
                    {
                        var result = data.Read<T>();
                        action?.Invoke(data);
                        return result;
                    }

                }
                catch (NpgsqlException ex)
                {
                    Log($"QUERY: {query}", LogType.Error);
                    Log("Query error: " + ex.ToString(), LogType.Error);

                    var incorrectSyntax = new List<string> { "42000", "42601" };

                    if (incorrectSyntax.Contains(ex.Code))
                    {
                        //Hide detail incorect syntax message. This may contain sensitive information
                        throw new IzendaException(Messages.FusionQueryInCorrectSyntax);
                    }

                    throw new FusionException(Messages.FusionCanNotQueryData);
                }
            }
        }
    
    }
}
