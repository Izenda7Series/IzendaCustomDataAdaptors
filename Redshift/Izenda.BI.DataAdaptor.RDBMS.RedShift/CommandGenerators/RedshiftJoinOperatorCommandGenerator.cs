using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.Constants;
using Izenda.BI.DataAdaptor.RDBMS.Redshift.Constants;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators
{
    public class RedshiftJoinOperatorCommandGenerator : JoinOperatorCommandGenerator
    {
        public RedshiftJoinOperatorCommandGenerator(QueryTreeCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        protected override SelectFieldCommandFormat SelectFieldCommandFormat
        {
            get
            {
                if (selectFieldCommandFormat == null)
                {
                    selectFieldCommandFormat = new RedshiftSelectFieldCommandFormat();
                }

                return selectFieldCommandFormat;
            }
        }
    }
}
