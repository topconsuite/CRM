using System;
using System.Collections.Specialized;

public static class NameValueCollectionExtensions
{
    public static string GetQueryStringValue(this NameValueCollection query, string key)
    {
        if (query == null || string.IsNullOrEmpty(key))
            return null;

        string value = query[key];
        if (value != null)
            return value;

        string rawQuery = query.ToString();
        var pairs = rawQuery.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var pair in pairs)
        {
            var kv = pair.Split(new[] { '=' }, 2);
            if (kv[0] == key)
                return kv.Length > 1 ? kv[1] : "";
        }

        return null;
    }
}
