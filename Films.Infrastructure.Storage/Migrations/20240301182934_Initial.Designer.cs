﻿// <auto-generated />
using System;
using Films.Infrastructure.Storage.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Films.Infrastructure.Storage.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240301182934_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CdnModelVoiceModel", b =>
                {
                    b.Property<long>("CdnModelId")
                        .HasColumnType("bigint");

                    b.Property<long>("VoicesId")
                        .HasColumnType("bigint");

                    b.HasKey("CdnModelId", "VoicesId");

                    b.HasIndex("VoicesId");

                    b.ToTable("CndVoices", (string)null);
                });

            modelBuilder.Entity("CountryModelFilmModel", b =>
                {
                    b.Property<long>("CountriesId")
                        .HasColumnType("bigint");

                    b.Property<Guid>("FilmModelId")
                        .HasColumnType("uuid");

                    b.HasKey("CountriesId", "FilmModelId");

                    b.HasIndex("FilmModelId");

                    b.ToTable("FilmCountries", (string)null);
                });

            modelBuilder.Entity("FilmModelGenreModel", b =>
                {
                    b.Property<Guid>("FilmModelId")
                        .HasColumnType("uuid");

                    b.Property<long>("GenresId")
                        .HasColumnType("bigint");

                    b.HasKey("FilmModelId", "GenresId");

                    b.HasIndex("GenresId");

                    b.ToTable("FilmGenres", (string)null);
                });

            modelBuilder.Entity("FilmModelPersonModel", b =>
                {
                    b.Property<long>("DirectorsId")
                        .HasColumnType("bigint");

                    b.Property<Guid>("FilmModelId")
                        .HasColumnType("uuid");

                    b.HasKey("DirectorsId", "FilmModelId");

                    b.HasIndex("FilmModelId");

                    b.ToTable("FilmDirectors", (string)null);
                });

            modelBuilder.Entity("FilmModelPersonModel1", b =>
                {
                    b.Property<Guid>("FilmModel1Id")
                        .HasColumnType("uuid");

                    b.Property<long>("ScreenwritersId")
                        .HasColumnType("bigint");

                    b.HasKey("FilmModel1Id", "ScreenwritersId");

                    b.HasIndex("ScreenwritersId");

                    b.ToTable("FilmScreenWriters", (string)null);
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Comment.CommentModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("FilmId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Country.CountryModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Film.CdnModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<Guid>("FilmId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Quality")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.ToTable("FilmCdns");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Film.FilmActorModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("FilmId")
                        .HasColumnType("uuid");

                    b.Property<long>("PersonId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("PersonId");

                    b.ToTable("FilmActors");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Film.FilmModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<int?>("CountEpisodes")
                        .HasColumnType("integer");

                    b.Property<int?>("CountSeasons")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("character varying(1500)");

                    b.Property<bool>("IsSerial")
                        .HasColumnType("boolean");

                    b.Property<string>("PosterUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double?>("RatingImdb")
                        .HasColumnType("double precision");

                    b.Property<double?>("RatingKp")
                        .HasColumnType("double precision");

                    b.Property<string>("ShortDescription")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<double>("UserRating")
                        .HasColumnType("double precision");

                    b.Property<int>("UserRatingsCount")
                        .HasColumnType("integer");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Films");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Genre.GenreModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Person.PersonModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Playlist.PlaylistFilmModel", b =>
                {
                    b.Property<Guid>("FilmId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PlaylistId")
                        .HasColumnType("uuid");

                    b.HasKey("FilmId", "PlaylistId");

                    b.HasIndex("PlaylistId");

                    b.ToTable("PlaylistFilms");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Playlist.PlaylistModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("PosterUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rating.RatingModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("FilmId")
                        .HasColumnType("uuid");

                    b.Property<double>("Score")
                        .HasColumnType("double precision");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rooms.BaseRoom.BannedModel<Films.Infrastructure.Storage.Models.Rooms.FilmRoom.FilmRoomModel>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoomId");

                    b.HasIndex("RoomId");

                    b.ToTable("BannedFilmViewers");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rooms.BaseRoom.BannedModel<Films.Infrastructure.Storage.Models.Rooms.YoutubeRoom.YoutubeRoomModel>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoomId");

                    b.HasIndex("RoomId");

                    b.ToTable("BannedYoutubeViewers");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rooms.BaseRoom.ViewerModel<Films.Infrastructure.Storage.Models.Rooms.FilmRoom.FilmRoomModel>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoomId");

                    b.HasIndex("RoomId");

                    b.ToTable("FilmViewers");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rooms.BaseRoom.ViewerModel<Films.Infrastructure.Storage.Models.Rooms.YoutubeRoom.YoutubeRoomModel>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoomId");

                    b.HasIndex("RoomId");

                    b.ToTable("YoutubeViewers");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rooms.FilmRoom.FilmRoomModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("CdnName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<Guid>("FilmId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ServerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("ServerId");

                    b.ToTable("FilmRooms");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rooms.YoutubeRoom.YoutubeRoomModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<Guid>("ServerId")
                        .HasColumnType("uuid");

                    b.Property<bool>("VideoAccess")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.ToTable("YoutubeRooms");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Server.ServerModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<int>("MaxRoomsCount")
                        .HasColumnType("integer");

                    b.Property<int>("RoomsCount")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.User.HistoryModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("FilmId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("UserId");

                    b.ToTable("UserHistory");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.User.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<bool>("Beep")
                        .HasColumnType("boolean");

                    b.Property<bool>("Change")
                        .HasColumnType("boolean");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Scream")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.User.WatchlistModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("FilmId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("UserId");

                    b.ToTable("UserWatchlist");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Voice.VoiceModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)");

                    b.HasKey("Id");

                    b.ToTable("Voices");
                });

            modelBuilder.Entity("GenreModelPlaylistModel", b =>
                {
                    b.Property<long>("GenresId")
                        .HasColumnType("bigint");

                    b.Property<Guid>("PlaylistModelId")
                        .HasColumnType("uuid");

                    b.HasKey("GenresId", "PlaylistModelId");

                    b.HasIndex("PlaylistModelId");

                    b.ToTable("PlaylistGenres", (string)null);
                });

            modelBuilder.Entity("GenreModelUserModel", b =>
                {
                    b.Property<long>("GenresId")
                        .HasColumnType("bigint");

                    b.Property<Guid>("UserModelId")
                        .HasColumnType("uuid");

                    b.HasKey("GenresId", "UserModelId");

                    b.HasIndex("UserModelId");

                    b.ToTable("UserGenres", (string)null);
                });

            modelBuilder.Entity("CdnModelVoiceModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Film.CdnModel", null)
                        .WithMany()
                        .HasForeignKey("CdnModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.Voice.VoiceModel", null)
                        .WithMany()
                        .HasForeignKey("VoicesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CountryModelFilmModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Country.CountryModel", null)
                        .WithMany()
                        .HasForeignKey("CountriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.Film.FilmModel", null)
                        .WithMany()
                        .HasForeignKey("FilmModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FilmModelGenreModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Film.FilmModel", null)
                        .WithMany()
                        .HasForeignKey("FilmModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.Genre.GenreModel", null)
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FilmModelPersonModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Person.PersonModel", null)
                        .WithMany()
                        .HasForeignKey("DirectorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.Film.FilmModel", null)
                        .WithMany()
                        .HasForeignKey("FilmModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FilmModelPersonModel1", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Film.FilmModel", null)
                        .WithMany()
                        .HasForeignKey("FilmModel1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.Person.PersonModel", null)
                        .WithMany()
                        .HasForeignKey("ScreenwritersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Comment.CommentModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Film.FilmModel", "Film")
                        .WithMany()
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.User.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Film");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Film.CdnModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Film.FilmModel", "Film")
                        .WithMany("CdnList")
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Film");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Film.FilmActorModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Film.FilmModel", "Film")
                        .WithMany("Actors")
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.Person.PersonModel", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Film");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Playlist.PlaylistFilmModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Film.FilmModel", "Film")
                        .WithMany()
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.Playlist.PlaylistModel", "Playlist")
                        .WithMany("Films")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Film");

                    b.Navigation("Playlist");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rating.RatingModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Film.FilmModel", "Film")
                        .WithMany()
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.User.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Film");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rooms.BaseRoom.BannedModel<Films.Infrastructure.Storage.Models.Rooms.FilmRoom.FilmRoomModel>", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Rooms.FilmRoom.FilmRoomModel", "Room")
                        .WithMany("Banned")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.User.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rooms.BaseRoom.BannedModel<Films.Infrastructure.Storage.Models.Rooms.YoutubeRoom.YoutubeRoomModel>", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Rooms.YoutubeRoom.YoutubeRoomModel", "Room")
                        .WithMany("Banned")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.User.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rooms.BaseRoom.ViewerModel<Films.Infrastructure.Storage.Models.Rooms.FilmRoom.FilmRoomModel>", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Rooms.FilmRoom.FilmRoomModel", "Room")
                        .WithMany("Viewers")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.User.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rooms.BaseRoom.ViewerModel<Films.Infrastructure.Storage.Models.Rooms.YoutubeRoom.YoutubeRoomModel>", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Rooms.YoutubeRoom.YoutubeRoomModel", "Room")
                        .WithMany("Viewers")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.User.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rooms.FilmRoom.FilmRoomModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Film.FilmModel", "Film")
                        .WithMany()
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.Server.ServerModel", "Server")
                        .WithMany()
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Film");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rooms.YoutubeRoom.YoutubeRoomModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Server.ServerModel", "Server")
                        .WithMany()
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Server");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.User.HistoryModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Film.FilmModel", "Film")
                        .WithMany()
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.User.UserModel", "User")
                        .WithMany("History")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Film");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.User.WatchlistModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Film.FilmModel", "Film")
                        .WithMany()
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.User.UserModel", "User")
                        .WithMany("Watchlist")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Film");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GenreModelPlaylistModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Genre.GenreModel", null)
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.Playlist.PlaylistModel", null)
                        .WithMany()
                        .HasForeignKey("PlaylistModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GenreModelUserModel", b =>
                {
                    b.HasOne("Films.Infrastructure.Storage.Models.Genre.GenreModel", null)
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Films.Infrastructure.Storage.Models.User.UserModel", null)
                        .WithMany()
                        .HasForeignKey("UserModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Film.FilmModel", b =>
                {
                    b.Navigation("Actors");

                    b.Navigation("CdnList");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Playlist.PlaylistModel", b =>
                {
                    b.Navigation("Films");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rooms.FilmRoom.FilmRoomModel", b =>
                {
                    b.Navigation("Banned");

                    b.Navigation("Viewers");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.Rooms.YoutubeRoom.YoutubeRoomModel", b =>
                {
                    b.Navigation("Banned");

                    b.Navigation("Viewers");
                });

            modelBuilder.Entity("Films.Infrastructure.Storage.Models.User.UserModel", b =>
                {
                    b.Navigation("History");

                    b.Navigation("Watchlist");
                });
#pragma warning restore 612, 618
        }
    }
}