using System;

namespace BookCollabSaaS.Domain.User;

public class UserEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
