namespace AuthService.Infrastructure.Web.Exceptions;

/// <summary>
/// Исключение, возникающее при отсутствии URL параметра
/// </summary>
public class QueryParameterMissingException(string param) : ArgumentException(
    $@"The string URL parameter {param} is missing", param);