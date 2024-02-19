using System;

namespace Films.Domain.Films.Exceptions;

public class EmptyCdnsCollectionException() : Exception("The cdns collection cannot be empty");