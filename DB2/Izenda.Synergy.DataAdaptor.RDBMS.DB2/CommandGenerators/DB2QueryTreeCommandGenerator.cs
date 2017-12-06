using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.Framework.Components.QueryExpressionTree;
using Izenda.BI.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Izenda.BI.DataAdaptor.RDBMS.DB2.CommandGenerators
{
    public class DB2QueryTreeCommandGenerator : QueryTreeCommandGenerator
    {
        /// <summary>
        /// Generate query for operand
        /// </summary>
        /// <param name="operand"></param>
        /// <param name="context"></param>
        /// <returns>
        /// The query
        /// </returns>
        protected override string GenerateCommand(QueryTreeNode operand, FusionContextData context)
        {
            var visitor = new DB2QueryTreeCommandGeneratorVisitor();
            visitor.ContextData = context;
            operand.Accept(visitor);

            return visitor.NodeData[operand.Id];
        }
    }
}
