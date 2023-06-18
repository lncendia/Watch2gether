namespace Overoom.Application.Abstractions.Users.DTOs;

public class EditUserDto
{
    public EditUserDto(Guid id, string? username, string? email)
    {
        Id = id;
        Username = username;
        Email = email;
    }

    public Guid Id { get; }
    public string? Username { get; }
    public string? Email { get; }
}