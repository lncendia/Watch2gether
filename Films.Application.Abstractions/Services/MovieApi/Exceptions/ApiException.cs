namespace Films.Application.Abstractions.Services.MovieApi.Exceptions;

public class ApiException(string message, Exception? inner) : Exception(message, inner);