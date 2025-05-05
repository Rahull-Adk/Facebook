namespace Facebook.DTOs;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }

    public ApiResponse(T data)
    {
        IsSuccess = true;
        Data = data;
        Message = string.Empty;
        Errors = new List<string>();
    }
    
    public ApiResponse(string message)
    {
        IsSuccess = false;
        Data = default;
        Message = message;
        Errors = new List<string> {message};
    }
}