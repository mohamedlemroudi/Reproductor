public class ServiceResult
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }

    public static ServiceResult Success(string? message = null)
    {
        return new ServiceResult { IsSuccess = true, Message = message };
    }

    public static ServiceResult Failure(string message)
    {
        return new ServiceResult { IsSuccess = false, Message = message };
    }
}