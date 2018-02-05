using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators.ConvertNullToEmptyGenerator;
using Izenda.BI.Framework.Models;
using System.Text;

namespace Izenda.BI.DataAdaptor.RDBMS.RedShift.CommandGenerators.ConvertNullToEmptyGenerator
{
    public class RedshiftNullToEmptyConvertor : NullToEmptyConvertor
    {
        public override StringBuilder GenerateSelectCommand(StringBuilder stringBuilder, string fieldAlias, QuerySourceField querySourceField = null)
        {
            if (querySourceField != null)
            {
                return stringBuilder.AppendFormat(@"NVL([[{0}]],'') AS ""{1}"",", querySourceField.Name, fieldAlias);
            }

            return stringBuilder.AppendFormat(@"NVL([[{0}]],'') AS ""{1}"",", fieldAlias, fieldAlias);
        }

        public override StringBuilder GenerateSelectCommandFormat(StringBuilder stringBuilder, string fieldAlias, QuerySourceField querySourceField = null)
        {
            if (querySourceField != null)
            {
                return stringBuilder.AppendFormat(@"NVL({0},'') AS ""{1}"",", querySourceField.Name, fieldAlias);
            }

            return stringBuilder.AppendFormat(@"NVL({0},'') AS ""{1}"",", fieldAlias, fieldAlias);
        }
    }
}
