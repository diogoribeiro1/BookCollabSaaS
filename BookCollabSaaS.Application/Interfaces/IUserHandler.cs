using System;
using BookCollabSaaS.Application.DTOs.User;

namespace BookCollabSaaS.Application.Interfaces;

public interface IUserHandler
{
    Task<UserResponse> CreateOrUpdateAsync(CreateUserRequest request);
    Task<IEnumerable<UserResponse>> GetAllAsync();
    Task<UserResponse> GetByIdAsync(Guid id);
}
