using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topsys.TopConWeb.SharedKernel.Common
{
    public class PagedList<T>
    {
        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int PageSize { get; set; }

        public int RecordCount { get; set; }

        public IEnumerable<T> Records { get; set; }

        public string Info {
            get
            {
                return "{"
                    + "\"currentPage\": " + CurrentPage 
                    + ", \"pageCount\": " + PageCount 
                    + ", \"pageSize\": " + PageSize 
                    + ", \"recordCount\": " + RecordCount
                    + "}";
            }
        }

    }
}
