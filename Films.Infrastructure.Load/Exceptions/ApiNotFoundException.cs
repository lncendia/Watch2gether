namespace Films.Infrastructure.Load.Exceptions;

public class ApiNotFoundException() : ApiException("The content you requested was not found", null);