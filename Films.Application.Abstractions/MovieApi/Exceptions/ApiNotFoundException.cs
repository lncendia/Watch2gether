namespace Films.Application.Abstractions.MovieApi.Exceptions;

public class ApiNotFoundException() : ApiException("The content you requested was not found", null);