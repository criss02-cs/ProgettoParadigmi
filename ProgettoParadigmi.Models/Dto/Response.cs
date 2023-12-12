namespace ProgettoParadigmi.Models.Dto
{
    public record Response<T>
    {
        public T? Result { get; set; }
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
    }

    public static class ResponseFactory
    {
        public static Response<T> CreateResponseFromResult<T>(T? result, bool isSuccess = true, string error = "")
        {
            return new Response<T>
            {
                Result = result,
                IsSuccess = isSuccess,
                Error = error
            };
        }
    }
}
