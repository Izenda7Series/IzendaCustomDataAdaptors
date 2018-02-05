using Izenda.BI.DataAdaptor.RDBMS.Constants;
using Izenda.BI.Framework.Constants;

namespace Izenda.BI.DataAdaptor.RDBMS.Redshift.Constants
{
    public class RedshiftSupportDataType : DatabaseSupportDataType
    {
        public RedshiftSupportDataType()
        {
            RegisterNumeric();
            RegisterDateTime();
            RegisterBoolean();
            RegisterText();
        }

        private void RegisterNumeric()
        {
            AddDatabaseDataType("numeric", IzendaDataType.NumericType, true, "System.Decimal", true /*default mapping*/);

            // integer
            AddDatabaseDataType("smallint", IzendaDataType.NumericType, true, "System.Int16");
            AddDatabaseDataType("int2", IzendaDataType.NumericType, true, "System.Int16");

            AddDatabaseDataType("integer", IzendaDataType.NumericType, true, "System.Int32");
            AddDatabaseDataType("int", IzendaDataType.NumericType, true, "System.Int32");
            AddDatabaseDataType("int4", IzendaDataType.NumericType, true, "System.Int32");

            AddDatabaseDataType("int8", IzendaDataType.NumericType, true, "System.Int64");
            AddDatabaseDataType("bigint", IzendaDataType.NumericType, true, "System.Int64");

            // floating point number
            AddDatabaseDataType("real", IzendaDataType.NumericType, true, "System.Single");
            AddDatabaseDataType("float4", IzendaDataType.NumericType, true, "System.Single");

            AddDatabaseDataType("float", IzendaDataType.NumericType, true, "System.Double");
            AddDatabaseDataType("float8", IzendaDataType.NumericType, true, "System.Double");
            AddDatabaseDataType("double precision", IzendaDataType.NumericType, true, "System.Double");

            AddDatabaseDataType("decimal", IzendaDataType.NumericType, true, "System.Decimal");
        }

        private void RegisterDateTime()
        {
            AddDatabaseDataType("date", IzendaDataType.DatetimeType, true, "System.DateTime", true/*default mapping*/);

            // timestamp is alias for [timestamp with time zone]
            AddDatabaseDataType("timestamp", IzendaDataType.DatetimeType, true, "System.DateTime");
            AddDatabaseDataType("timestamp with time zone", IzendaDataType.DatetimeType, true, "System.DateTime");

            // timestamptz is alias for [timestamp without time zone]
            AddDatabaseDataType("timestamptz", IzendaDataType.DatetimeType, true, "System.DateTime");
            AddDatabaseDataType("timestamp without time zone", IzendaDataType.DatetimeType, true, "System.DateTime");
        }

        private void RegisterBoolean()
        {
            // bool is alias for boolean
            AddDatabaseDataType("bool", IzendaDataType.BooleanType, true, "System.Boolean");
            AddDatabaseDataType("boolean", IzendaDataType.BooleanType, true, "System.Boolean", true/*default mapping*/);
        }

        private void RegisterText()
        {
            // 4k range
            AddDatabaseDataType("char", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("character", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("nchar", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("national character", IzendaDataType.TextType, true, "System.String");

            // 64k range
            AddDatabaseDataType("varchar", IzendaDataType.TextType, true, "System.String", true/*default mapping*/);
            AddDatabaseDataType("character varying", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("nvarchar", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("national character varying", IzendaDataType.TextType, true, "System.String");

            // 265 bytes
            AddDatabaseDataType("bpchar", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("text", IzendaDataType.TextType, true, "System.String");
        }
    }
}
