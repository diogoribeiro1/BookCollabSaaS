using System;

namespace BookCollabSaaS.Application.DTOs.User;

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
