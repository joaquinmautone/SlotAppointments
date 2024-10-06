namespace SlotAppointments.Services.Communication
{
    public class ApiResponse<T>
    {
        public bool Success { get; protected set; }
        public string Message { get; protected set; }
        public T Data { get; set; }

        public ApiResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public ApiResponse(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
