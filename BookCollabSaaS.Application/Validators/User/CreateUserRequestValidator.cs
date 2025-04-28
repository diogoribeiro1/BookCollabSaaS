using System;
using BookCollabSaaS.Application.DTOs.User;

namespace BookCollabSaaS.Application.Validators;

public static class CreateUserRequestValidator
{
    public static void ValidateCreateRequest(CreateUserRequest request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (string.IsNullOrWhiteSpace(request.Name)) throw new ArgumentException("Name is required.", nameof(request.Name));
        if (string.IsNullOrWhiteSpace(request.Email)) throw new ArgumentException("Email is required.", nameof(request.Email));
    }
}
