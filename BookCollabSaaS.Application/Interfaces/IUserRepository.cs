using BookCollabSaaS.Domain.User;

namespace BookCollabSaaS.Application.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<UserEntity>> GetAllAsync();

    Task<UserEntity> GetByIdAsync(Guid id);

    Task AddAsync(UserEntity user);
}
