namespace BookCollabSaaS.Application.DTOs.User;

public class UserResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<string> Roles { get; set; } = new();

    public UserResponse()
    {

    }

}
