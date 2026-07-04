namespace EliteEvents.Application.Common;

/// <summary>
/// Standard API response wrapper for consistent response format.
/// </summary>
/// <typeparam name="T">The data type being returned.</typeparam>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation successful")
    {
        return new ApiResponse<T> { Success = true, Message = message, Data = data };
    }

    public static ApiResponse<T> FailResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse<T> { Success = false, Message = message, Errors = errors };
    }
}

/// <summary>
/// Non-generic API response for operations that don't return data.
/// </summary>
public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string>? Errors { get; set; }

    public static ApiResponse SuccessResponse(string message = "Operation successful")
    {
        return new ApiResponse { Success = true, Message = message };
    }

    public static ApiResponse FailResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse { Success = false, Message = message, Errors = errors };
    }
}
