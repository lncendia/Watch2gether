using System;

namespace Films.Domain.Ratings.Exceptions;

public class ScoreException() : Exception("Rating must be between 0 and 10");