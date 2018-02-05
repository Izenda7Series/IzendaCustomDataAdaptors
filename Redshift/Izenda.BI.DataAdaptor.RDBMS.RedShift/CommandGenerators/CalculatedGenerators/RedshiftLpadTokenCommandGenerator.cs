using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.Framework.Components.ExpressionEvaluations;
using Izenda.BI.Framework.Components.ExpressionEvaluations.Functions;
using System;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators.CalculatedGenerators
{
    public class RedshiftLpadTokenCommandGenerator : LpadTokenCommandGenerator
    {
        public RedshiftLpadTokenCommandGenerator(ExpressionCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        public override string GenerateSelfCommand(Token token)
        {
            var functionToken = token as DatabaseFunctionToken;
            var subTrees = functionToken.SubTrees;

            string expression = visitor.NodeData[(subTrees["1"] as Token).TokenId];
            var length = visitor.NodeData[(subTrees["2"] as Token).TokenId];
            var paddingCharactor = visitor.NodeData[(subTrees["3"] as Token).TokenId];
            var lengthValue = Convert.ToInt32(length);


            return string.Format("LPAD({0},{1},{2})", expression, length, paddingCharactor);
        }
    }
}
