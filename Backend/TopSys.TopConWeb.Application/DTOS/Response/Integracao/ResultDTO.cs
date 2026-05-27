using System.Collections.Generic;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application.DTOS.Response.Integracao
{
    public class ResultDTO<T>
    {

        public ResultDTO(EResultDTOStatus status, string message, T result, string errorCode = "")
        {
            _status = status;
            Message = message;
            Result = result;
            ErrorCode = errorCode;
        }

        public ResultDTO(EResultDTOStatus status, string message, string errorCode = "")
        {
            _status = status;
            Message = message;
            ErrorCode = errorCode;
        }

        public ResultDTO(EResultDTOStatus status, string message, List<Error> errors, string errorCode = "")
        {
            _status = status;
            Message = message;
            Errors = errors;
            ErrorCode = errorCode;

            if (ErrorCode == "") ErrorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_PRECONDITION_FAILED.GetMessageCode();
        }

        private EResultDTOStatus _status { get; set; }
        public string Status { 
            get {
                switch(_status)
                {
                    case EResultDTOStatus.Success:
                        return "success";
                    case EResultDTOStatus.Alert:
                        return "alert";
                    case EResultDTOStatus.Error:
                        return "error";
                    default:
                        return "success";
                }
                    
            }
        }

        public string Message { get; set; }
        public T Result { get; set; }
        public string ErrorCode { get; set; }
        public List<Error> Errors { get; set; }

    }

    public enum EResultDTOStatus
    {
        Success,
        Alert,
        Error
    }

    public class Error
    {
        public Error(string errorCode, string message, int? recordPosition = null)
        {
            ErrorCode = errorCode;
            Message = message;
            RecordPosition = recordPosition;
        }

        public Error() {}

        public int? RecordPosition { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
