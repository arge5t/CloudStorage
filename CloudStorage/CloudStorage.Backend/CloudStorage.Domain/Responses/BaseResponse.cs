using CloudStorage.Domain.Enums;

namespace CloudStorage.Domain.Responses
{
    public class BaseResponse<T>
    {
        public string? Message { get; set; }
        public StatusCode StatusCode { get; set; }
        public T? Data { get; set; }
    }
}
