using Izenda.BI.DataAdaptor.RDBMS.Constants;
using Izenda.BI.Framework.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Izenda.BI.DataAdaptor.RDBMS.ODBC.Constants
{
    public class ODBCSupportDataType : DatabaseSupportDataType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ODBCSupportDataType()
        {
            // Datetime
            AddDatabaseDataType("SQL_TYPE_TIMES", IzendaDataType.DatetimeType, true, "System.DateTime");
            AddDatabaseDataType("SQL_TYPE_TIMESTAMP", IzendaDataType.DatetimeType, true, "System.DateTime");

            // Numeric
            AddDatabaseDataType("SQL_DECIMAL", IzendaDataType.NumericType, true, "System.Decimal");
            AddDatabaseDataType("SQL_DOUBLE", IzendaDataType.NumericType, true, "System.Double");
            AddDatabaseDataType("SQL_REAL", IzendaDataType.NumericType, true, "System.Single");
            AddDatabaseDataType("SQL_NUMERIC", IzendaDataType.NumericType, true, "System.Decimal");
            AddDatabaseDataType("SQL_BIGINT", IzendaDataType.NumericType, true, "System.Int64");
            AddDatabaseDataType("SQL_INTEGER", IzendaDataType.NumericType, true, "System.Int32");
            AddDatabaseDataType("SQL_SMALLINT", IzendaDataType.NumericType, true, "System.Int16");
            AddDatabaseDataType("SQL_TINYINT", IzendaDataType.NumericType, true, "System.Byte");

            // Boolean
            AddDatabaseDataType("SQL_BIT", IzendaDataType.BooleanType, true, "System.Boolean");

            // Lob
            AddDatabaseDataType("SQL_BINARY", IzendaDataType.LobType, false, "System.Byte[]");
            AddDatabaseDataType("SQL_LONGVARBINARY", IzendaDataType.LobType, false, "System.Byte[]");
            AddDatabaseDataType("SQL_WCHAR", IzendaDataType.LobType, false, "System.String");
            AddDatabaseDataType("SQL_WLONGVARCHAR", IzendaDataType.LobType, false, "System.String");
            AddDatabaseDataType("SQL_WVARCHAR", IzendaDataType.LobType, false, "System.Byte[]");
            AddDatabaseDataType("SQL_VARBINARY", IzendaDataType.LobType, false, "System.Byte[]");

            // Text
            AddDatabaseDataType("SQL_CHAR", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("SQL_LONG_VARCHAR", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("SQL_WCHAR", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("SQL_WLONGVARCHAR", IzendaDataType.TextType, true, "System.String");
            AddDatabaseDataType("SQL_WVARCHAR", IzendaDataType.TextType, true, "System.String");

            // Guid
            AddDatabaseDataType("SQL_GUID", IzendaDataType.TextType, false, "System.Guid");
        }
    }
}
