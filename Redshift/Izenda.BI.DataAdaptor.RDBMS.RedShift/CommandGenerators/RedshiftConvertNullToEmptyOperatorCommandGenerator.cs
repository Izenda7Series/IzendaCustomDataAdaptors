using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators
{
    public class RedshiftConvertNullToEmptyOperatorCommandGenerator : ConvertNullToEmptyOperatorCommandGenerator
    {
        public RedshiftConvertNullToEmptyOperatorCommandGenerator(QueryTreeCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        public override SelectFieldCommandGenerator SelectFieldCommandGenerator
        {
            get
            {
                return new RedshiftSelectFieldCommandGenerator(visitor);
            }
        }
    }
}
