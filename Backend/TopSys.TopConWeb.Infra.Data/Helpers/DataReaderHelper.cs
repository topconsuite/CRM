using System;
using System.Data;

namespace TopSys.TopConWeb.Infra.Data.Helpers
{
    public static class DataReaderHelper
    {
        public static T GetValue<T>(this IDataReader dataReader, string columnName)
        {
            return (T)Convert.ChangeType(dataReader.GetValue(dataReader.GetOrdinal(columnName)), typeof(T));
        }
    }
}
