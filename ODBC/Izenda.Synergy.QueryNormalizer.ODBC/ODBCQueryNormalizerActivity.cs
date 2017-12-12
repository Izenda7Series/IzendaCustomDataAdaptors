﻿using Izenda.BI.Framework.Components.SequenceWorkflows;
using Izenda.BI.Framework.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Izenda.BI.QueryNormalizer.ODBC
{
    public abstract class ODBCQueryNormalizerActivity : IActivity<QueryNormalizerContext>
    {
        public abstract int Order { get;  }

        public abstract void Execute(QueryNormalizerContext context);
    }
}
