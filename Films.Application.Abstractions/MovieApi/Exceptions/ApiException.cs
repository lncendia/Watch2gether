namespace Films.Application.Abstractions.MovieApi.Exceptions;

public class ApiException(string message, Exception? inner) : Exception(message, inner);