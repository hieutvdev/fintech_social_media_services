using ShredKernel.Aggregates;

namespace User.Domain.Models;

public class UserType : Entity<int>, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    
    public string Description { get; private set; } = default!;
    
    public static UserType Create(string name, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        
        var userType =  new UserType
        {
            Name = name,
            Description = description
        };

        return userType;
    }
    
    public void Update(string name, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        
        Name = name;
        Description = description;
    }
    
}