﻿using Watch2gether.Domain.Abstractions.Exceptions;
using Watch2gether.Domain.Abstractions.Interfaces;
using Watch2gether.Domain.Abstractions.Repositories.UnitOfWorks;
using Watch2gether.Domain.Films.Enums;
using Watch2gether.Domain.Films.Exceptions;
using Watch2gether.Domain.Rooms.FilmRoom;

namespace Watch2gether.Domain.Services.Services;

public class FilmRoomService : IFilmRoomService
{
    private readonly IUnitOfWork _unitOfWork;

    public FilmRoomService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task ChangeSeasonAsync(FilmRoom room, Guid viewerId, int season)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(room.FilmId);
        if (film!.Type != FilmType.Serial) throw new NotSerialException();
        if (film.FilmData.CountSeasons < season) throw new SeasonException();
        //room.ChangeSeason(viewerId, season);
    }

    public async Task ChangeSeriesAsync(FilmRoom room, Guid viewerId, int series)
    {
        var film = await _unitOfWork.FilmRepository.Value.GetAsync(room.FilmId);
        if (film!.Type != FilmType.Serial) throw new NotSerialException();
        if (film.FilmData.CountEpisodes < series) throw new SeriesException();
        //room.ChangeSeason(viewerId, series);
    }
}