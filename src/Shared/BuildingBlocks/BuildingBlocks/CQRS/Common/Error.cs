namespace BuildingBlocks.CQRS.Common;

public class Error : IEquatable<Error>
{
    public static readonly Error None = new Error(string.Empty, string.Empty);
    public static readonly Error NullValue = new Error("Error.NullValue", "Value cannot be null.");
    public static readonly Error Unexpected = new Error("Error.Unexpected", "An unexpected error occurred.");

    // Validation Errors
    public static readonly Error InvalidValue = new Error("Error.InvalidValue", "The value provided is invalid.");
    public static readonly Error RequiredField = new Error("Error.RequiredField", "This field is required.");
    public static readonly Error OutOfRange = new Error("Error.OutOfRange", "The value is out of the allowed range.");
    public static readonly Error TooShort = new Error("Error.TooShort", "The value is too short.");
    public static readonly Error TooLong = new Error("Error.TooLong", "The value is too long.");
    public static readonly Error InvalidEmail = new Error("Error.InvalidEmail", "The email address is invalid.");

    // Authentication & Authorization Errors
    public static readonly Error Unauthorized = new Error("Error.Unauthorized", "Unauthorized access.");
    public static readonly Error Forbidden = new Error("Error.Forbidden", "Access to this resource is forbidden.");
    public static readonly Error TokenExpired = new Error("Error.TokenExpired", "The authentication token has expired.");
    public static readonly Error InvalidCredentials = new Error("Error.InvalidCredentials", "The provided credentials are invalid.");

    // Database Errors
    public static readonly Error RecordNotFound = new Error("Error.RecordNotFound", "The requested record was not found.");
    public static readonly Error DuplicateRecord = new Error("Error.DuplicateRecord", "The record already exists.");
    public static readonly Error DatabaseError = new Error("Error.DatabaseError", "A database error occurred.");
    public static readonly Error ConcurrencyConflict = new Error("Error.ConcurrencyConflict", "A concurrency conflict occurred.");
    
    // Network Errors
    public static readonly Error NetworkFailure = new Error("Error.NetworkFailure", "A network error occurred.");
    public static readonly Error ServiceUnavailable = new Error("Error.ServiceUnavailable", "The service is unavailable.");

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }

    public static implicit operator string(Error error) => error.Code;

    public override bool Equals(object? obj)
    {
        if (obj is Error other)
        {
            return Equals(other);
        }

        return false;
    }

    public bool Equals(Error? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Code == other.Code && Message == other.Message;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Message);
    }

    public static bool operator ==(Error? a, Error? b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Error? a, Error? b)
    {
        return !(a == b);
    }

}
