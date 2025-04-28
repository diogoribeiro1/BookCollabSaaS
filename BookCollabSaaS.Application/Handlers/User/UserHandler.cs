using BookCollabSaaS.Application.DTOs.User;
using BookCollabSaaS.Application.Interfaces;
using BookCollabSaaS.Application.Mappers.User;
using BookCollabSaaS.Application.Validators;
using BookCollabSaaS.Infrastructure.Data.Repositories.Interfaces;

namespace BookCollabSaaS.Application.Handlers
{
    public class UserHandler : IUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITokenService _tokenService;
        private readonly ICacheService _cacheService;

        public UserHandler(IUserRepository userRepository, ITokenService tokenService, IRoleRepository roleRepository, ICacheService cacheService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));

        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest request)
        {
            CreateUserRequestValidator.ValidateCreateRequest(request);

            var user = UserMapper.ToEntity(request);

            var role = await _roleRepository.GetByNameAsync("Admin")
                ?? throw new Exception("Role 'Admin' not found.");

            user.AddRole(role);

            await _userRepository.AddAsync(user);

            return UserMapper.ToResponse(user);
        }

        public async Task<string> GenerateTokenAsync(LoginRequest request)
        {
            LoginRequestValidator.ValidateLoginRequest(request);

            var user = await _userRepository.GetByEmailAsync(request.Email)
                ?? throw new UnauthorizedAccessException("Invalid email or password.");

            if (!user.VerifyPassword(request.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var role = user.Roles.FirstOrDefault()?.Name ?? "User";

            return _tokenService.GenerateToken(user.Id.ToString(), role);
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(UserMapper.ToResponse);
        }

        public async Task<UserResponse> GetByIdAsync(Guid id)
        {
            var cacheKey = $"user:{id}";
            var cachedUser = await _cacheService.GetAsync<UserResponse>(cacheKey);

            if (cachedUser != null)
            {
                return cachedUser;
            }

            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new Exception("User not found.");

            var userResponse = UserMapper.ToResponse(user);

            await _cacheService.SetAsync(cacheKey, userResponse, TimeSpan.FromHours(1)); // cache for 1 hour

            return userResponse;
        }


        public Task<UserResponse> UpdateAsync(UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
