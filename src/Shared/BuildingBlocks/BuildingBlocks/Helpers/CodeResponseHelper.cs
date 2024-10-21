using BuildingBlocks.CQRS.Common;
using BuildingBlocks.Exceptions.ErrorCodeResponse;

namespace BuildingBlocks.Helpers;

public static class CodeResponseHelper
{
    private static readonly List<Error> Errors = HttpStatusCode.Messages
        .Select(kv => new Error(kv.Key.ToString(), kv.Value))
        .ToList();
    
    public static List<Error> GetAllErrors() => Errors;
    
    public static Error GetErrorByCode(int code)
    {
        var error = Errors.FirstOrDefault(e => e.Code == code.ToString());
        return error ?? Error.None; 
    }
}