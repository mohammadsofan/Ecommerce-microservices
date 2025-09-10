namespace ProductService.Application.Wrappers
{
    public class Error
    {
        public string Field { get; set; } = string.Empty;   
        public string Message { get; set; } = string.Empty;
    }
    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<Error>? Errors { get; set; }
        public int StatusCode { get; set; }
        public static ServiceResult<T> Ok(int statusCode, T data, string? message = null)
        {
            return new ServiceResult<T>
            {
                Data = data,
                Success = true,
                Message = message,
                StatusCode = statusCode
            };
        }
        public static ServiceResult<T> Fail(int statusCode, string? message = null, List<Error>? errors = null)
        {
            return new ServiceResult<T>
            {
                Data = default,
                Success = false,
                Message = message,
                Errors = errors,
                StatusCode = statusCode
            };
        }
    }
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<Error>? Errors { get; set; }
        public int StatusCode { get; set; }
        public static ServiceResult Ok(int statusCode, string? message = null)
        {
            return new ServiceResult
            {
                Success = true,
                Message = message,
                StatusCode = statusCode
            };
        }
        public static ServiceResult Fail(int statusCode, string? message = null, List<Error>? errors = null)
        {
            return new ServiceResult
            {
                Success = false,
                Message = message,
                Errors = errors,
                StatusCode = statusCode
            };
        }
    }
}
