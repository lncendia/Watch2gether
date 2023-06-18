namespace Overoom.Application.Abstractions.Users.DTOs;

public class UserCreateDto
{
    public UserCreateDto(string username, string password, string email)
    {
        Username = username;
        Password = password;
        Email = email;
    }

    public string Username { get; }
    public string Password { get; }
    public string Email { get; }
}