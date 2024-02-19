using System;

namespace Films.Domain.Comments.Exceptions;

public class TextLengthException() : Exception("Text length must be between 1 and 1000 characters");