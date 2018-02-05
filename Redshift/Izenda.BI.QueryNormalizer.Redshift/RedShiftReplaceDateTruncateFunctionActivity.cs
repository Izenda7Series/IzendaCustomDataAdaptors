using Izenda.BI.Framework.Models.Contexts;

namespace Izenda.BI.QueryNormalizer.Redshift
{
    public class RedShiftReplaceDateTruncateFunctionActivity
        : RedshiftQueryNormalizerActivity
    {
        public override int Order => 12;

        public override void Execute(QueryNormalizerContext context)
        {
            var sql = context.Query;
            sql = sql.Replace(@"DATETRUNCATE", @"DATE_TRUNC");

            context.Query = sql;
        }
    }
}
