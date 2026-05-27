using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topsys.TopConWeb.SharedKernel.Helpers
{
    public static class DateHelper
    {
        public static DateTime? ExtractFromIDD(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            int lastSpaceIndex = input.LastIndexOf(' ');

            if (lastSpaceIndex <= 0 || lastSpaceIndex == input.Length - 1)
            {
                return null;
            }

            string namePart = input.Substring(0, lastSpaceIndex);
            string datePart = input.Substring(lastSpaceIndex + 1);

            if (DateTime.TryParseExact(datePart, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }

            return null;
        }
    }
}
