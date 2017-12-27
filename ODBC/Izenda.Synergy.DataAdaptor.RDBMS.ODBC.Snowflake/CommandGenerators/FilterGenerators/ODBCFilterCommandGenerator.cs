using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators.FilterGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Izenda.BI.Framework.Models;
using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBC.CommandGenerators.FilterGenerators
{
    public class ODBCFilterCommandGenerator : FilterCommandGenerator
    {
        public ODBCFilterCommandGenerator(FusionContextData contextData) : base(contextData)
        {
        }

        protected override string DatabasePrefix => "ODBC";

        public override NameFormatter NameFormatter => new ODBCNameFormatter();        
    }
}
