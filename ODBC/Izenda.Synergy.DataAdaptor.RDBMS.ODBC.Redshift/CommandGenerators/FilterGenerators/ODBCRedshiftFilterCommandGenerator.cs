using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators.FilterGenerators;
using Izenda.BI.Framework.Models;
using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.ODBC.CommandGenerators;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBCRedshift.CommandGenerators.FilterGenerators
{
    public class ODBCRedshiftFilterCommandGenerator : FilterCommandGenerator
    {
        public ODBCRedshiftFilterCommandGenerator(FusionContextData contextData) : base(contextData)
        {
        }

        protected override string DatabasePrefix => "ODBCRedshift";

        public override NameFormatter NameFormatter => new ODBCNameFormatter();        
    }
}
