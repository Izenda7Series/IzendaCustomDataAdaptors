using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.Constants;

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

        public override DateAddTokenCommandGenerator DateAddTokenCommandGenerator
        {
            get
            {
                return new RedshiftDateAddTokenCommandGenerator(null);
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
