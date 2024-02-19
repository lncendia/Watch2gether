using System;

namespace Films.Domain.Playlists.Exceptions;

public class EmptyGenresCollectionException() : Exception("The collection of genres cannot be empty");