using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators.CalculatedGenerators;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators.CalculatedGenerators
{
    public class RedshiftExpressionCommandGeneratorVisitor : ExpressionCommandGeneratorVisitor
    {
        public override ConvertTokenCommandGenerator ConvertTokenCommandGenerator
        {
            get
            {
                return new RedshiftConvertTokenCommandGenerator(this);
            }
        }

        public override CastTokenCommandGenerator CastTokenCommandGenerator
        {
            get
            {
                return new RedshiftCastTokenCommandGenerator(this);
            }
        }

        public override DatePartTokenCommandGenerator DatePartTokenCommandGenerator
        {
            get
            {
                return new RedshiftDatePartTokenCommandGenerator(this);
            }
        }

        public override IsNullTokenCommandGenerator IsNullTokenCommandGenerator
        {
            get
            {
                return new RedshiftIsNullTokenCommandGenerator(this);
            }
        }

        public override MappingTokenCommandGenerator MappingTokenCommandGenerator
        {
            get
            {
                return new RedshiftMappingTokenCommandGenerator(this);
            }
        }

        public override LpadTokenCommandGenerator LpadTokenCommandGenerator
        {
            get
            {
                return new RedshiftLpadTokenCommandGenerator(this);
            }
        }
    }
}
