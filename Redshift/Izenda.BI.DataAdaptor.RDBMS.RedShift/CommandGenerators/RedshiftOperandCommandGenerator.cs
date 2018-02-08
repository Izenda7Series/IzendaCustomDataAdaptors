using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.Constants;
using Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators.CalculatedGenerators;
using Izenda.BI.DataAdaptor.RDBMS.Redshift.Constants;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators
{
    public class RedshiftOperandCommandGenerator : OperandCommandGenerator
    {

        private SelectFieldCommandFormat selectFieldCommandFormat;

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

        public override CastTokenCommandGenerator CastTokenCommandGenerator
        {
            get
            {
                return new RedshiftCastTokenCommandGenerator(null);
            }
        }

        public RedshiftOperandCommandGenerator(QueryTreeCommandGeneratorVisitor visitor) : base(visitor)
        {
        }
    }
}
