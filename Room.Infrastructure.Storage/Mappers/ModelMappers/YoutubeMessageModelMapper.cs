using Microsoft.EntityFrameworkCore;
using Room.Domain.Messages.YoutubeMessages;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.Messages;
using Room.Infrastructure.Storage.Models.YoutubeRooms;

namespace Room.Infrastructure.Storage.Mappers.ModelMappers;

internal class YoutubeMessageModelMapper(ApplicationDbContext context)
    : IModelMapperUnit<MessageModel<YoutubeRoomModel>, YoutubeMessage>
{
    public async Task<MessageModel<YoutubeRoomModel>> MapAsync(YoutubeMessage aggregate)
    {
        var model = await context.YoutubeMessages
            .FirstOrDefaultAsync(x => x.Id == aggregate.Id) ?? new MessageModel<YoutubeRoomModel> { Id = aggregate.Id };

        model.UserId = aggregate.UserId;
        model.RoomId = aggregate.RoomId;
        model.Text = aggregate.Text;
        model.CreatedAt = aggregate.CreatedAt;

        return model;
    }
}