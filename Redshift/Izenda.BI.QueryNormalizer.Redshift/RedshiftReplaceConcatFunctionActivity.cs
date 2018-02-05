using System.Text;
using Izenda.BI.Framework.Models.Contexts;
using Izenda.BI.QueryNormalizer.Utility;

namespace Izenda.BI.QueryNormalizer.Redshift
{
    public class RedshiftReplaceConcatFunctionActivity 
        : RedshiftQueryNormalizerActivity
    {
        public override int Order => 10;

        public override void Execute(QueryNormalizerContext context)
        {
            var sql = context.Query;
            int index = -1;
            var concat = "IZENDA_CONCAT";

            index = sql.IndexOf(concat);

            while (index >= 0)
            {
                var openIndex = sql.IndexOf("(", index);
                var closeIndex = ConcatFunctionUtil.FindMatchCloseIndex(sql, openIndex + 1);

                var fieldValues = sql.Substring(openIndex + 1, closeIndex - 1 - openIndex);
                var fields = ConcatFunctionUtil.GetConcatParams(fieldValues);
                var builder = new StringBuilder();

                for (int i = 0; i < fields.Count; i++)
                {
                    builder.Append($"CAST({fields[i].Trim()} AS TEXT)");

                    if (i < fields.Count - 1)
                    {
                        builder.Append(" || ");
                    }
                }

                var replacedContent = builder.ToString();
                var originalContent = sql.Substring(index, closeIndex - index + 1);
                sql = sql.Replace(originalContent, replacedContent);

                index = sql.IndexOf(concat);
            }

            context.Query = sql;
        }
    }
}
