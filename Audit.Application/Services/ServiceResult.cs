using Audit.Shared.Services;

namespace Audit.Application.Services
{
    public class ServiceResult<TData> : IServiceResult
    {
        public ServiceResult()
        {
            
        }

        public ServiceResult(bool success, TData? data, string message = "")
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public bool Success { get; set; }
        public TData? Data { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
