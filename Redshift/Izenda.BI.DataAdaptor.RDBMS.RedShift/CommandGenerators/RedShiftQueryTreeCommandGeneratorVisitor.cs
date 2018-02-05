using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;
using Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators
{
    public class RedshiftQueryTreeCommandGeneratorVisitor : QueryTreeCommandGeneratorVisitor
    {
        public override PagingOperatorCommandGenerator PagingOperatorCommandGenerator
        {
            get
            {
                return new RedshiftPagingOperatorCommandGenerator(this);
            }
        }

        public override OperandCommandGenerator OperandCommandGenerator
        {
            get
            {
                return new RedshiftOperandCommandGenerator(this);
            }
        }

        public override ProjectionOperatorCommandGenerator ProjectionOperatorCommandGenerator
        {
            get
            {
                return new RedshiftProjectionOperatorCommandGenerator(this);
            }
        }

        public override SelectionOperatorCommandGenerator SelectionOperatorCommandGenerator
        {
            get
            {
                return new RedshiftSelectionOperatorCommandGenerator(this);
            }
        }

        public override GroupingOperatorCommandGenerator GroupingOperatorCommandGenerator
        {
            get
            {
                return new RedshiftGroupingOperatorCommandGenerator(this);
            }
        }

        public override ResultLimitOperatorCommandGenerator ResultLimitOperatorCommandGenerator
        {
            get
            {
                return new RedshiftResultLimitOperatorCommandGenerator(this);
            }
        }

        public override JoinOperatorCommandGenerator JoinOperatorCommandGenerator
        {
            get
            {
                return new RedshiftJoinOperatorCommandGenerator(this);
            }
        }

        public override ConvertNullToEmptyOperatorCommandGenerator ConvertNullToEmptyOperatorCommandGenerator
        {
            get
            {
                return new RedshiftConvertNullToEmptyOperatorCommandGenerator(this);
            }
        }
    }
}
