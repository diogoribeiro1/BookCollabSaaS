using BookCollabSaaS.Application.DTOs.User;
using BookCollabSaaS.Application.Handlers;
using BookCollabSaaS.Application.Interfaces;
using BookCollabSaaS.Domain.User;
using BookCollabSaaS.Infrastructure.Data.Repositories.Interfaces;
using Moq;

namespace BookCollabSaaS.Tests.User;

public class UserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly UserHandler _userHandler;

    public UserHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _roleRepositoryMock = new Mock<IRoleRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _cacheServiceMock = new Mock<ICacheService>();

        _userHandler = new UserHandler(
            _userRepositoryMock.Object,
            _tokenServiceMock.Object,
            _roleRepositoryMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateUserAndReturnResponse()
    {
        // Arrange
        var request = new CreateUserRequest { Email = "test@example.com", Password = "Password123" };
        var role = new RoleEntity("Admin");

        _roleRepositoryMock.Setup(r => r.GetByNameAsync("Admin"))
            .ReturnsAsync(role);

        _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<UserEntity>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _userHandler.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Email, result.Email);
        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<UserEntity>()), Times.Once);
    }

    [Fact]
    public async Task GenerateTokenAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LoginRequest { Email = "test@example.com", Password = "Password123" };
        var user = new UserEntity("test@example.com", "hashedPassword");
        user.SetPassword(request.Password); // Assuming SetPassword hashes the password internally
        user.AddRole(new RoleEntity("Admin"));

        _userRepositoryMock.Setup(u => u.GetByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _tokenServiceMock.Setup(t => t.GenerateToken(It.IsAny<string>(), "Admin"))
            .Returns("valid_token");

        // Act
        var token = await _userHandler.GenerateTokenAsync(request);

        // Assert
        Assert.Equal("valid_token", token);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_FromCache()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var cachedUser = new UserResponse { Id = userId, Email = "cached@example.com" };

        _cacheServiceMock.Setup(c => c.GetAsync<UserResponse>($"user:{userId}"))
            .ReturnsAsync(cachedUser);

        // Act
        var result = await _userHandler.GetByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cachedUser.Email, result.Email);
        _userRepositoryMock.Verify(u => u.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_FromDatabase_WhenNotInCache()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User("db@example.com", "password");
        user.Id = userId;

        _cacheServiceMock.Setup(c => c.GetAsync<UserResponse>($"user:{userId}"))
            .ReturnsAsync((UserResponse)null);

        _userRepositoryMock.Setup(u => u.GetByIdAsync(userId))
            .ReturnsAsync(user);

        _cacheServiceMock.Setup(c => c.SetAsync($"user:{userId}", It.IsAny<UserResponse>(), It.IsAny<TimeSpan>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _userHandler.GetByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("db@example.com", result.Email);
        _userRepositoryMock.Verify(u => u.GetByIdAsync(userId), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync($"user:{userId}", It.IsAny<UserResponse>(), It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<UserEntity>
            {
                new UserEntity("user1@example.com", "pass1"),
                new UserEntity("user2@example.com", "pass2")
            };

        _userRepositoryMock.Setup(u => u.GetAllAsync())
            .ReturnsAsync(users);

        // Act
        var result = await _userHandler.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
}
