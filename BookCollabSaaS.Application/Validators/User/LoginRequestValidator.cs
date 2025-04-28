using System;
using BookCollabSaaS.Application.DTOs.User;

namespace BookCollabSaaS.Application.Validators;

public static class LoginRequestValidator
{
    public static void ValidateLoginRequest(LoginRequest request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (string.IsNullOrWhiteSpace(request.Email)) throw new ArgumentException("Email is required.", nameof(request.Email));
        if (string.IsNullOrWhiteSpace(request.Password)) throw new ArgumentException("Password is required.", nameof(request.Password));
    }
}
