﻿namespace Overoom.Application.Abstractions.Film.Kinopoisk.DTOs;

public class Film
{
    public Film(long kpId, string? imdbId, string title, int year, bool serial, string? description,
        string? shortDescription, string posterUrl, double? ratingKp, double? ratingImdb,
        IReadOnlyCollection<string> countries, IReadOnlyCollection<string> genres)
    {
        KpId = kpId;
        ImdbId = imdbId;
        Title = title;
        Serial = serial;
        Description = description;
        ShortDescription = shortDescription;
        PosterUrl = posterUrl;
        RatingKp = ratingKp;
        RatingImdb = ratingImdb;
        Countries = countries;
        Genres = genres;
        Year = year;
    }

    public long KpId { get; }
    public string? ImdbId { get; }
    public string Title { get; }
    public int Year { get; }
    public bool Serial { get; }
    public string? Description { get; }
    public string? ShortDescription { get; }
    public string PosterUrl { get; }
    public double? RatingKp { get; }
    public double? RatingImdb { get; }
    public IReadOnlyCollection<string> Countries { get; }
    public IReadOnlyCollection<string> Genres { get; }
}