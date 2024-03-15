using Microsoft.EntityFrameworkCore;
using Room.Domain.Messages.FilmMessages;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.FilmRooms;
using Room.Infrastructure.Storage.Models.Messages;

namespace Room.Infrastructure.Storage.Mappers.ModelMappers;

internal class FilmMessageModelMapper(ApplicationDbContext context)
    : IModelMapperUnit<MessageModel<FilmRoomModel>, FilmMessage>
{
    public async Task<MessageModel<FilmRoomModel>> MapAsync(FilmMessage aggregate)
    {
        var model = await context.FilmMessages
            .FirstOrDefaultAsync(x => x.Id == aggregate.Id) ?? new MessageModel<FilmRoomModel> { Id = aggregate.Id };

        model.UserId = aggregate.UserId;
        model.RoomId = aggregate.RoomId;
        model.Text = aggregate.Text;
        model.CreatedAt = aggregate.CreatedAt;

        return model;
    }
}