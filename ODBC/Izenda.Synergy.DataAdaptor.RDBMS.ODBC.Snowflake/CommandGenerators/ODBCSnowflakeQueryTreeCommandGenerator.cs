using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.Framework.Components.QueryExpressionTree;
using Izenda.BI.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBC.CommandGenerators
{
    public class ODBCSnowflakeQueryTreeCommandGenerator : ODBCQueryTreeCommandGenerator
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
            var visitor = new ODBCSnowflakeQueryTreeCommandGeneratorVisitor();
            visitor.ContextData = context;
            operand.Accept(visitor);

            return visitor.NodeData[operand.Id];
        }

        /// <summary>
        /// Applies the advanced setting.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void ApplyAdvancedSetting(FusionContextData context)
        {

        }
    }
}
