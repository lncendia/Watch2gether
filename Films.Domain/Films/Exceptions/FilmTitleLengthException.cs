using System;

namespace Films.Domain.Films.Exceptions;

public class FilmTitleLengthException() : Exception("The movie name length must be between 1 and 200 characters");