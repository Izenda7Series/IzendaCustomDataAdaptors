using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBC.CommandGenerators
{
    public class ODBCValueTokenCommandGenerator : ValueTokenCommandGenerator
    {
        public ODBCValueTokenCommandGenerator(ExpressionCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        public override NameFormatter NameFormatter => new ODBCNameFormatter();
    }
}
