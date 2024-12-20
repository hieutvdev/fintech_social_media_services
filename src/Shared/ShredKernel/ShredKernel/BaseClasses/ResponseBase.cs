namespace ShredKernel.BaseClasses;

public class ResponseBase
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public object? Metadata { get; set; }

    public ResponseBase()
    {
        
    }

    public ResponseBase(bool success, string message, int statusCode, object? metadata)
    {
        Success = success;
        Message = message;
        StatusCode = statusCode;
        Metadata = metadata;
    }

    public static ResponseBase CreateSuccessResponse(string message = "Successful", int statusCode = 200,
        object? metaData = null)
    {
        return new(true, message, statusCode, metaData);
    }

    public static ResponseBase CreateSuccessResponseData(object? metaData)
    {
        return CreateSuccessResponse(metaData: metaData);
    }

    public static ResponseBase CreateFailureResponse(string message = "Failed", int statusCode = 400,
        object? metaData = null)
    {
        return new(false, message, statusCode, metaData);
    }
}