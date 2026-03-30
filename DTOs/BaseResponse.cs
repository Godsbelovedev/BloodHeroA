namespace BloodHeroA.DTOs
{
    public class BaseResponse<T>
    {
        public string? Message { get; set; }
        public bool Status { get; set; }
        public T? Data { get; set; }

       public static BaseResponse<T> Success(T? data, string message = "Success") =>
       new() { Status = true, Message = message, Data = data };

        public static BaseResponse<T> Failure(string message) =>
        new() { Status = false, Message = message };
    }
}
