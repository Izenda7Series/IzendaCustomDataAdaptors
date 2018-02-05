using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators.CalculatedGenerators
{
    public class RedshiftExpressionCommandGenerator : ExpressionCommandGenerator
    {
        public override ExpressionCommandGeneratorVisitor ExpressionCommandGeneratorVisitor
        {
            get
            {
                return new RedshiftExpressionCommandGeneratorVisitor();
            }
        }
    }
}
