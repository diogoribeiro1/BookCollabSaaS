using System;
using BookCollabSaaS.Application.DTOs.User;
using BookCollabSaaS.Domain.User;

namespace BookCollabSaaS.Application.Mappers.User;

public static class UserMapper
{
    public static UserEntity ToEntity(CreateUserRequest request)
    {
        return new UserEntity(
            name: request.Name,
            email: request.Email,
            password: request.Password
        );
    }

    public static UserResponse ToResponse(UserEntity user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Roles = user.Roles?.Select(r => r.Name).ToList()
        };
    }
}

