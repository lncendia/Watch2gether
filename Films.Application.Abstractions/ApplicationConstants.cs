namespace Films.Application.Abstractions;

public static class ApplicationConstants
{
    public const string ProjectName = "Films.Start";
    public const string RoomScheme = "Room";
    public const string AdminRoleName = "admin";
    public const string AvatarClaimType = "Avatar";
    public static readonly Uri DefaultAvatar = new("img/avatars/default.jpg", UriKind.Relative);
    public static readonly Uri DefaultPoster = new("img/posters/default.jpg", UriKind.Relative);
}