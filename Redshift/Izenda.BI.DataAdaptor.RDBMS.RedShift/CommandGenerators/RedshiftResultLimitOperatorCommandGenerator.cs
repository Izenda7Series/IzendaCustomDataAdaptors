using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.Framework.Components.QueryExpressionTree;
using Izenda.BI.Framework.Components.QueryExpressionTree.Operator;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators
{
    public class RedshiftResultLimitOperatorCommandGenerator : ResultLimitOperatorCommandGenerator
    {
        public RedshiftResultLimitOperatorCommandGenerator(QueryTreeCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        public override string GenerateCommand(QueryTreeNode treeNode, string childCommand)
        {
            var resultLimitOperator = treeNode as ResultLimitOperator;
            var query = @"SELECT * FROM ({0}) X LIMIT {1}";
            var resultLimit = string.Format(query, childCommand, resultLimitOperator.Limit);
            visitor.ContextData.TempData["resultLimit"] = resultLimit;
            return resultLimit;
        }
    }
}
