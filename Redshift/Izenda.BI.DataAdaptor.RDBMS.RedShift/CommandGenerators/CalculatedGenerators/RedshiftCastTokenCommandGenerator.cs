using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.Constants;
using Izenda.BI.DataAdaptor.RDBMS.Redshift.Constants;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators.CalculatedGenerators
{
    public class RedshiftCastTokenCommandGenerator : CastTokenCommandGenerator
    {
        public RedshiftCastTokenCommandGenerator(ExpressionCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        public override DatabaseSupportDataType DatabaseSupportDataType
        {
            get
            {
                return new RedshiftSupportDataType();
            }
        }
    }
}
