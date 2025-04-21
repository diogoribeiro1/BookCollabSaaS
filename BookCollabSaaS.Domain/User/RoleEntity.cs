using System;

namespace BookCollabSaaS.Domain.User;

public class RoleEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private RoleEntity() { }

    public RoleEntity(string name)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
