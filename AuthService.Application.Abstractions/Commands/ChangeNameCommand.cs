using AuthService.Application.Abstractions.Entities;
using MediatR;

namespace AuthService.Application.Abstractions.Commands;

public class ChangeNameCommand : IRequest<UserData>
{
    public required long Id { get; init; }
    public required string Name { get; init; }
}