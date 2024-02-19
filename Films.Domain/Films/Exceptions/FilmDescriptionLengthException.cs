using System;

namespace Films.Domain.Films.Exceptions;

public class FilmDescriptionLengthException()
    : Exception("The movie description length must be between 1 and 1500 characters");