using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators.CalculatedGenerators
{
    public class RedshiftMappingTokenCommandGenerator : MappingTokenCommandGenerator
    {
        public RedshiftMappingTokenCommandGenerator(ExpressionCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        public override string ServerTypeId
        {
            get
            {
                return "E285BFD1-F8D5-4BEB-A345-B3D2EF5A3DE8";
            }
        }
    }
}
