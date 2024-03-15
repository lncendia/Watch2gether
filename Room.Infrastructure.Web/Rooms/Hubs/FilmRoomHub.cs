using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Room.Application.Abstractions.Commands.FilmRooms;
using Room.Application.Abstractions.Exceptions;
using Room.Application.Abstractions.Queries.FilmRooms;
using Room.Domain.Messages.Messages.Exceptions;
using Room.Domain.Rooms.FilmRooms.Exceptions;
using Room.Domain.Rooms.Rooms.Exceptions;
using Room.Infrastructure.Web.Rooms.Mappers;
using Room.Infrastructure.Web.Rooms.ViewModels;
using Room.Infrastructure.Web.Rooms.ViewModels.Messages;

namespace Room.Infrastructure.Web.Rooms.Hubs;

[Authorize]
public class FilmRoomHub(IMediator mediator) : Hub
{
    public async Task Connect(Guid roomId)
    {
        try
        {
            await mediator.Send(new SetOnlineCommand
            {
                RoomId = roomId,
                Online = true,
                ViewerId = GetUserId()
            });

            Context.Items.Add("roomId", roomId);

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

            var room = await mediator.Send(new FilmRoomByIdQuery { Id = roomId });

            await Clients.Caller.SendAsync("Room", FilmRoomMapper.Map(room));
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    public async Task GetMessages(Guid? fromMessageId, int count = 50)
    {
        if (count > 50) count = 50;

        try
        {
            var userId = GetUserId();
            var roomId = GetRoomId();
            var (messages, total) = await mediator.Send(new FilmRoomMessagesQuery
            {
                RoomId = roomId,
                FromMessageId = fromMessageId,
                Count = count,
                ViewerId = userId
            });

            await Clients.Caller.SendAsync("Messages", new MessagesViewModel
            {
                Messages = messages.Select(MessageMapper.Map).ToArray(),
                Count = total
            });
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }


    public async Task ChangeSeries(int season, int series)
    {
        try
        {
            var userId = GetUserId();
            var roomId = GetRoomId();
            await mediator.Send(new ChangeSeriesCommand
            {
                Season = season,
                Series = series,
                ViewerId = userId,
                RoomId = roomId
            });
            await Clients.OthersInGroup(roomId.ToString()).SendAsync("ChangeSeries", userId, season, series);
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    public async Task SendMessage(string message)
    {
        try
        {
            var userId = GetUserId();
            var roomId = GetRoomId();

            await mediator.Send(new SendMessageCommand
            {
                Message = message,
                ViewerId = userId,
                RoomId = roomId
            });

            await Clients.OthersInGroup(roomId.ToString()).SendAsync("NewMessage", userId, message);
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    public async Task SetTimeLine(int seconds)
    {
        try
        {
            var userId = GetUserId();
            var roomId = GetRoomId();
            await mediator.Send(new SetTimeLineCommand
            {
                TimeLine = TimeSpan.FromSeconds(seconds),
                ViewerId = userId,
                RoomId = roomId
            });
            await Clients.OthersInGroup(roomId.ToString()).SendAsync("Seek", userId, seconds);
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    public async Task SetPause(bool pause, int seconds)
    {
        try
        {
            var userId = GetUserId();
            var roomId = GetRoomId();
            await mediator.Send(new SetPauseCommand
            {
                Pause = pause,
                ViewerId = userId,
                RoomId = roomId
            });
            await mediator.Send(new SetTimeLineCommand
            {
                TimeLine = TimeSpan.FromSeconds(seconds),
                ViewerId = userId,
                RoomId = roomId
            });
            await Clients.OthersInGroup(roomId.ToString()).SendAsync("Pause", userId, pause, seconds);
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    public async Task SetFullScreen(bool fullScreen)
    {
        try
        {
            var userId = GetUserId();
            var roomId = GetRoomId();
            await mediator.Send(new SetFullscreenCommand
            {
                Fullscreen = fullScreen,
                ViewerId = userId,
                RoomId = roomId
            });
            await Clients.OthersInGroup(roomId.ToString()).SendAsync("FullScreen", userId, fullScreen);
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    public async Task Beep(Guid target)
    {
        try
        {
            var userId = GetUserId();
            var roomId = GetRoomId();
            await mediator.Send(new BeepCommand
            {
                ViewerId = userId,
                RoomId = roomId,
                TargetId = target
            });
            await Clients.OthersInGroup(roomId.ToString()).SendAsync("Beep", userId, target);
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    public async Task Scream(Guid target)
    {
        try
        {
            var userId = GetUserId();
            var roomId = GetRoomId();
            await mediator.Send(new ScreamCommand
            {
                ViewerId = userId,
                RoomId = roomId,
                TargetId = target
            });
            await Clients.OthersInGroup(roomId.ToString()).SendAsync("Scream", userId, target);
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }


    public async Task Kick(Guid target)
    {
        try
        {
            var userId = GetUserId();
            var roomId = GetRoomId();
            await mediator.Send(new KickCommand
            {
                ViewerId = userId,
                RoomId = roomId,
                TargetId = target
            });
            await Clients.OthersInGroup(roomId.ToString()).SendAsync("Kick", userId, target);
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }


    public async Task ChangeName(Guid target, string name)
    {
        try
        {
            var userId = GetUserId();
            var roomId = GetRoomId();
            await mediator.Send(new ChangeNameCommand
            {
                Name = name,
                ViewerId = userId,
                RoomId = roomId,
                TargetId = target
            });
            await Clients.OthersInGroup(roomId.ToString()).SendAsync("ChangeName", userId, target, name);
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    private Guid GetUserId()
    {
        var id = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException();
        return Guid.Parse(id);
    }

    private Guid GetRoomId()
    {
        return (Guid)(Context.Items["roomId"] ?? throw new InvalidOperationException());
    }

    private Task HandleException(Exception ex)
    {
        var error = ex switch
        {
            RoomNotFoundException => "Комната не найдена",
            ViewerNotFoundException => "Зритель не найден",
            ActionNotAllowedException => "Действие не разрешено",
            InvalidUsernameLengthException => "Длина имени должна составлять от 1 до 200 символов",
            ChangeFilmSeriesException => "Вы не можете изменить серию у фильма",
            MessageLengthException => "Сообщение должно быть от одного до тысячи символов",
            _ => "Неизвестная ошибка"
        };
        return Clients.Caller.SendAsync("Error", error);
    }
}