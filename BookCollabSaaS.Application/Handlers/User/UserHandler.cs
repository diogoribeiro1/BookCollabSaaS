using System;
using BookCollabSaaS.Application.DTOs.User;
using BookCollabSaaS.Application.Interfaces;
using BookCollabSaaS.Domain.User;

namespace BookCollabSaaS.Application.Handlers;

public class UserHandler(IUserRepository userRepository) : IUserHandler
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<UserResponse> CreateOrUpdateAsync(CreateUserRequest request)
    {
        var user = new UserEntity(request.Name);

        await _userRepository.AddAsync(user);

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name
        };
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(user => new UserResponse
        {
            Id = user.Id,
            Name = user.Name
        });
    }

    public async Task<UserResponse> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name
        };
    }
}
