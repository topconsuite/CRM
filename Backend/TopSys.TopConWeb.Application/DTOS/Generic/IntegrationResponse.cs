using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Generic
{
    public class IntegrationsResponse<TResult>
    {
        public string Status { get; set; } //"success" | "alert" | "IsError"
        public string Message { get; set; }
        public TResult Result { get; set; }
        public string ErrorCode { get; set; }
    }
}
