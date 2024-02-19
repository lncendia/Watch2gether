using System;

namespace Films.Domain.Films.Exceptions;

public class EmptyTagsCollectionException() : Exception("The tag collection cannot be empty");