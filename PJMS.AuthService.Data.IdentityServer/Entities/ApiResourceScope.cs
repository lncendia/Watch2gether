namespace PJMS.AuthService.Data.IdentityServer.Entities;

public class ApiResourceScope
{
    public int Id { get; set; }
    public string Scope { get; set; }

    public int ApiResourceId { get; set; }
    public ApiResource ApiResource { get; set; }
}