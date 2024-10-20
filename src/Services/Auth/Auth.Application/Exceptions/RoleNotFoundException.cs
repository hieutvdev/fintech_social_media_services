namespace Auth.Application.Exceptions;

public class RoleNotFoundException(string message) : NotFoundException($"{nameof(RoleNotFoundException)}: {message}");