﻿using Watch2gether.Infrastructure.PersistentStorage.Models.Rooms.Base;

namespace Watch2gether.Infrastructure.PersistentStorage.Models.Rooms;

public class FilmRoomModel : RoomBaseModel
{
    public Guid FilmId { get; set; }
    public List<FilmViewerModel> Viewers { get; set; } = new();
}