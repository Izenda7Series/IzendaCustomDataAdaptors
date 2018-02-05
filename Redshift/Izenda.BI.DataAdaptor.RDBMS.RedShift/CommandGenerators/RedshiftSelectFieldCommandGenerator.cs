using Izenda.BI.DataAdaptor.RDBMS.CommandGenerators;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.CommandGenerators
{
    public class RedshiftSelectFieldCommandGenerator : SelectFieldCommandGenerator
    {
        public RedshiftSelectFieldCommandGenerator(QueryTreeCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        protected override string SpecificNamespaceForConvertNullToEmpty
        {
            get
            {
                return string.Format(ConvertNullToEmptyNamespace, ".Redshift", ".Redshift", this.GetType().Assembly.GetName().Name);
            }
        }
    }
}
