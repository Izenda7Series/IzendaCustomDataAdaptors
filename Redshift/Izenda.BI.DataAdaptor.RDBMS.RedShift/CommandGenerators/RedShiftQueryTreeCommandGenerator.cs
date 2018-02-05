using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.Framework.Components.QueryExpressionTree;
using Izenda.BI.Framework.Models;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators
{
    public class RedshiftQueryTreeCommandGenerator : QueryTreeCommandGenerator
    {
        protected override string GenerateCommand(QueryTreeNode operand, FusionContextData context)
        {
            var visitor = new RedshiftQueryTreeCommandGeneratorVisitor();
            visitor.ContextData = context;
            operand.Accept(visitor);

            return visitor.NodeData[operand.Id];
        }

        protected override void ApplyAdvancedSetting(FusionContextData context)
        {

        }
    }
}
