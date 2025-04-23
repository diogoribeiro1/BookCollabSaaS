using System;
using BookCollabSaaS.Application.DTOs.User;
using BookCollabSaaS.Application.Interfaces;
using BookCollabSaaS.Domain.User;
using BookCollabSaaS.Infrastructure.Data.Repositories.Interfaces;

namespace BookCollabSaaS.Application.Handlers
{
    public class UserHandler(IUserRepository userRepository, ITokenService tokenService, IRoleRepository roleRepository) : IUserHandler
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<UserResponse> CreateOrUpdateAsync(CreateUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name)) throw new ArgumentNullException(nameof(request.Name));
            if (string.IsNullOrWhiteSpace(request.Email)) throw new ArgumentNullException(nameof(request.Email));

            var user = new UserEntity(
                name: request.Name,
                email: request.Email,
                password: request.Password
            );

            var role = await _roleRepository.GetByNameAsync("Admin");

            user.AddRole(role);

            await _userRepository.AddAsync(user);

            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<string> GenerateTokenAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !user.VerifyPassword(request.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var role = user.Roles.FirstOrDefault()?.Name ?? "User";

            return _tokenService.GenerateToken(user.Id.ToString(), role);
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(user => new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Roles = user.Roles.Select(r => r.Name).ToList()

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
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
