namespace LibraryManagement.API.Common;

// Standard response envelope used across all controllers so every endpoint
// returns a consistent shape, regardless of which module/person built it.
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string? message = null) =>
        new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> FailResponse(string message, List<string>? errors = null) =>
        new() { Success = false, Message = message, Errors = errors };
}
