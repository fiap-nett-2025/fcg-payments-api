namespace FCG.Payments.Application.DTO
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public required string Message { get; set; }
        public T? Data { get; set; }
    }
}
