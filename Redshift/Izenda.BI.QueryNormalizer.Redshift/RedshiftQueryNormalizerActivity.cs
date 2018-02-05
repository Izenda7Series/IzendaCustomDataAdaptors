using Izenda.BI.Framework.Components.SequenceWorkflows;
using Izenda.BI.Framework.Models.Contexts;

namespace Izenda.BI.QueryNormalizer.Redshift
{
    public abstract class RedshiftQueryNormalizerActivity
        : IActivity<QueryNormalizerContext>
    {
        public abstract int Order { get; }

        public abstract void Execute(QueryNormalizerContext context);
    }
}
