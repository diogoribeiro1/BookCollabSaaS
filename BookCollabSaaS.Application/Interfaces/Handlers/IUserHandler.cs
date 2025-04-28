using BookCollabSaaS.Application.DTOs.User;

namespace BookCollabSaaS.Application.Interfaces;

public interface IUserHandler
{
    Task<UserResponse> CreateAsync(CreateUserRequest request);
    Task<UserResponse> UpdateAsync(UpdateUserRequest request);
    Task<IEnumerable<UserResponse>> GetAllAsync();
    Task<UserResponse> GetByIdAsync(Guid id);
    Task<string> GenerateTokenAsync(LoginRequest request);
}
