using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators.FilterGenerators;
using Izenda.BI.Framework.Models;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators.FilterGenerators
{
    public class RedshiftFilterCommandGenerator : FilterCommandGenerator
    {
        public RedshiftFilterCommandGenerator(FusionContextData contextData) : base(contextData)
        {
        }

        protected override string DatabasePrefix
        {
            get
            {
                return "Redshift";
            }
        }
    }
}
