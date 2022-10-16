﻿using Watch2gether.Application.Abstractions.DTO.Rooms;

namespace Watch2gether.Application.Abstractions.Interfaces.Rooms;

public interface IRoomManager
{
    Task<ViewerDto> ConnectAsync(Guid roomId, string name);
    Task<ViewerDto> ConnectForUserAsync(Guid roomId, string email);
    Task SendMessageAsync(Guid roomId, Guid viewerId, string message);
    Task SetOnlineAsync(Guid roomId, Guid viewerId, bool online);
    Task SetPauseAsync(Guid roomId, Guid viewerId, bool pause, TimeSpan time);
}