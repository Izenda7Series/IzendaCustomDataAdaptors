using Izenda.BI.DataAdaptor.RDBMS.Constants;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.Constants
{
    public class RedshiftDatabaseFunction : DatabaseFunction
    {
        public override string DatePart
        {
            get
            {
                return "date_part";
            }
        }

        public override string IsNull
        {
            get
            {
                return "nvl";
            }
        }
    }
}
