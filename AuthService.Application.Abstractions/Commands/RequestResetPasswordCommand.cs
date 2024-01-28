using MediatR;

namespace AuthService.Application.Abstractions.Commands;

public class RequestResetPasswordCommand : IRequest
{
    public required string Email { get; init; }
    public required string ResetUrl { get; init; }
}