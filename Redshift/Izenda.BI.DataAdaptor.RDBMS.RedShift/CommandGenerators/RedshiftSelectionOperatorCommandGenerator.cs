using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators
{
    public class RedshiftSelectionOperatorCommandGenerator : SelectionOperatorCommandGenerator
    {
        public RedshiftSelectionOperatorCommandGenerator(QueryTreeCommandGeneratorVisitor visitor) : base(visitor)
        {
        }
    }
}
