namespace PJMS.AuthService.Web.Exceptions;

/// <summary>
/// Исключение, возникающее при отсутствии контекста IdentityServer.
/// </summary>
public class IdentityContextException() : Exception("Action is not possible without an identity context");