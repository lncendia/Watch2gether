using System;

namespace Films.Domain.Films.Exceptions;

public class EmptyVoicesCollectionException() : Exception("The collection of voices cannot be empty");