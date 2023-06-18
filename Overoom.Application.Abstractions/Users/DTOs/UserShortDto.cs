namespace Overoom.Application.Abstractions.Users.DTOs;

public class UserShortDto
{
    public UserShortDto(string username, string email, Guid id)
    {
        Username = username;
        Email = email;
        Id = id;
    }

    public Guid Id { get; }
    public string Username { get; }
    public string Email { get; }
}