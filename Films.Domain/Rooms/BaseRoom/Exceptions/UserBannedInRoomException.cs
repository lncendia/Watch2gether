using System;

namespace Films.Domain.Rooms.BaseRoom.Exceptions;

public class UserBannedInRoomException() : Exception("The viewer is blocked");