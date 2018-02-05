using Izenda.BI.Framework.Models.Contexts;

namespace Izenda.BI.QueryNormalizer.Redshift
{
    public class RedshiftEscapseTableNameActivity : RedshiftQueryNormalizerActivity
    {
        /// <summary>
        /// The activity order
        /// </summary>
        public override int Order
        {
            get
            {
                return 20;
            }
        }

        /// <summary>
        /// Execute the activity
        /// </summary>
        /// <param name="context">The context</param>
        public override void Execute(QueryNormalizerContext context)
        {
            var sql = context.Query;
            sql = sql.Replace("[[", @"""").Replace("]]", @"""");

            context.Query = sql;
        }
    }
}
