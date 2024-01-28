using MediatR;

namespace AuthService.Application.Abstractions.Commands;

public class ResetPasswordCommand : IRequest
{
    public required Guid Id { get; init; }
    public required string Code { get; init; }
    public required string NewPassword { get; init; }
}