using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.Constants;
using Izenda.BI.DataAdaptor.RDBMS.Redshift.Constants;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators.CalculatedGenerators
{
    public class RedshiftIsNullTokenCommandGenerator : IsNullTokenCommandGenerator
    {
        public RedshiftIsNullTokenCommandGenerator(ExpressionCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        public override DatabaseFunction DatabaseFunction
        {
            get
            {
                return new RedshiftDatabaseFunction();
            }
        }
    }
}
