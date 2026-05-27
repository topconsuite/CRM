using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topsys.TopConWeb.SharedKernel.Validation
{
    public class IntegrationValidatorError : Exception
    {
        public IntegrationValidatorError(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
