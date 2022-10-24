namespace Overoom.Application.Abstractions.DTO.Users;

public class UserDto
{
    public UserDto(string username, string password, string email)
    {
        Username = username;
        Password = password;
        Email = email;
    }

    public string Username { get; }
    public string Password { get; }
    public string Email { get; }
}