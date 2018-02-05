using Izenda.BI.DataAdaptor.RDBMS.Constants;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.Constants
{
    public class RedshiftDatabaseFunctionFormat : DatabaseFunctionFormat
    {
        public override string CheckBlankFormatText
        {
            get
            {
                return @"[[{0}]] = ''";
            }
        }

        public override string CheckNotBlankFormat
        {
            get
            {
                return "([[{0}]] IS NULL OR [[{0}]] <> '')";
            }
        }

        public override string CheckNotBlankFormatText
        {
            get
            {
                return "([[{0}]] IS NULL OR [[{0}]] <> '')";
            }
        }
    }
}
