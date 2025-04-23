using System;

namespace BookCollabSaaS.Application.DTOs.User;

public class CreateUserRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string FirebaseToken { get; set; } = string.Empty;

}
