using System;

namespace BookCollabSaaS.Domain.User;

public class UserEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private UserEntity() { }

    public UserEntity(string name)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public void UpdateName(string newName)
    {
        Name = newName ?? throw new ArgumentNullException(nameof(newName));
    }
}
