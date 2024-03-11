namespace Films.Infrastructure.Load.Exceptions;

public class ApiException(string message, Exception? inner) : Exception(message, inner);