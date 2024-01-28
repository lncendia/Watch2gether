namespace AuthService.Application.Abstractions.Exceptions;

public class EmailNotConfirmedException() : Exception("For this action, the email must be confirmed");