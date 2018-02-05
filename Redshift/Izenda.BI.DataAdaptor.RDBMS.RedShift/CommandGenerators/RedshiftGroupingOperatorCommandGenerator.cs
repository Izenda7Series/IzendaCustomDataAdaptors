using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators
{
    public class RedshiftGroupingOperatorCommandGenerator : GroupingOperatorCommandGenerator
    {
        public RedshiftGroupingOperatorCommandGenerator(QueryTreeCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        public override string DefaultGroupBy
        {
            get
            {
                return string.Empty;

            }
        }
    }
}
