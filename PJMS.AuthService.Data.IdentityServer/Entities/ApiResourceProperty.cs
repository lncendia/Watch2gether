namespace PJMS.AuthService.Data.IdentityServer.Entities;

public class ApiResourceProperty : Property
{
    public int ApiResourceId { get; set; }
    public ApiResource ApiResource { get; set; }
}