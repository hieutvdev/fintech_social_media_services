using Auth.Domain.Exceptions;

namespace Auth.Domain.ValueObjects;

public class UserSessionId
{
    public Guid Value { get; }
    private UserSessionId(Guid value) => Value = value;

    public static UserSessionId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("UserSessionId cannot be empty.");
        }

        return new UserSessionId(value);
    }
}