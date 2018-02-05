using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.Framework.Components.QueryExpressionTree;
using Izenda.BI.Framework.Components.QueryExpressionTree.Operator;
using Izenda.BI.Framework.Constants;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators
{
    public class RedshiftPagingOperatorCommandGenerator : PagingOperatorCommandGenerator
    {
        public RedshiftPagingOperatorCommandGenerator(QueryTreeCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        public override string GenerateCommand(QueryTreeNode treeNode, string childCommand)
        {
            var pagingOperator = (PagingOperator)treeNode;

            // Modify sortingOperator and selectOperator
            childCommand = childCommand.Replace(PlaceHolder.PagingField, "");

            var query = @"SELECT * FROM({0}) x LIMIT {1} OFFSET {2}";
            var paging = visitor.ContextData.Paging;
            int offset = (paging.PageIndex - 1) * paging.PageSize;

            return string.Format(query, childCommand, paging.PageSize, offset);
        }
    }
}
