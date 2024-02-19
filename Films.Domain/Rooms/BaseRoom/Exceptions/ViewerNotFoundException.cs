using System;

namespace Films.Domain.Rooms.BaseRoom.Exceptions;

public class ViewerNotFoundException() : Exception("A viewer is not found in this room.");