using AuthService.Application.Abstractions.Entities;
using MediatR;

namespace AuthService.Application.Abstractions.Commands;

public class ChangeAvatarCommand : IRequest<UserData>
{
    public required long Id { get; init; }
    public required Stream Avatar { get; init; }
}