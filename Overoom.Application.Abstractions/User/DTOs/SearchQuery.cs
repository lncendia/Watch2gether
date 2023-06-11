namespace Overoom.Application.Abstractions.User.DTOs;

public class SearchQuery
{
    public SearchQuery(int page, string? name, string? email)
    {
        Page = page;
        Name = name;
        Email = email;
    }

    public int Page { get; }
    public string? Name { get; }
    public string? Email { get; }
}