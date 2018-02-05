using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.Constants;
using Izenda.BI.DataAdaptor.RDBMS.Redshift.Constants;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators.CalculatedGenerators
{
    public class RedshiftDatePartTokenCommandGenerator : DatePartTokenCommandGenerator
    {
        public RedshiftDatePartTokenCommandGenerator(ExpressionCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        public override DatabaseFunction DatabaseFunction
        {
            get
            {
                return new RedshiftDatabaseFunction();
            }
        }

        public override DatabaseConstants DatabaseConstants
        {
            get
            {
                return new RedshiftDatabaseConstants();
            }
        }
    }
}
