namespace Overoom.Application.Abstractions;

public static class ApplicationConstants
{
    public const string ProjectName = "Overoom";
    public const string RoomScheme = "RoomTemporary";
    public const string AdminRoleName = "admin";
    public const string AvatarClaimType = "Avatar";
    public static readonly Uri DefaultAvatar = new("default.jpg", UriKind.Relative);
}