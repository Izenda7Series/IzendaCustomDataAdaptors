using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators.FilterGenerators;

namespace Izenda.BI.DataAdaptor.RDBMS.RedShift.CommandGenerators.FilterGenerators
{
    public class RedshiftDateTimeFilterGenerator : DateTimeFilterGenerator
    {
        protected override string DatabasePrefix
        {
            get
            {
                return "Redshift";
            }
        }
    }
}
