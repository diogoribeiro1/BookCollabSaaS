using System;

namespace BookCollabSaaS.Application.DTOs.User;

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
