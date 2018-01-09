using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators.FilterGenerators;
using Izenda.BI.Framework.Models;
using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.ODBC.CommandGenerators;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBCSnowflake.CommandGenerators.FilterGenerators
{
    public class ODBCSnowflakeFilterCommandGenerator : FilterCommandGenerator
    {
        public ODBCSnowflakeFilterCommandGenerator(FusionContextData contextData) : base(contextData)
        {
        }

        protected override string DatabasePrefix => "ODBCSnowflake";

        public override NameFormatter NameFormatter => new ODBCNameFormatter();        
    }
}
