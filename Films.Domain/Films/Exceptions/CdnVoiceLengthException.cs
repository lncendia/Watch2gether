namespace Films.Domain.Films.Exceptions;

public class CdnVoiceLengthException()
    : Exception("The CDN voice length must be between 1 and 60 characters");