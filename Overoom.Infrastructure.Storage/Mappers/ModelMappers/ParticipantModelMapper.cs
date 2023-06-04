using Overoom.Infrastructure.Storage.Mappers.Abstractions;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class ParticipantModelMapper : IModelMapperUnit<ParticipantModel, Participant>
{
    private readonly ApplicationDbContext _context;

    public ParticipantModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<ParticipantModel> MapAsync(Participant model)
    {
        var participant = await _context.Participants.FirstOrDefaultAsync(x => x.Id == model.Id) ??
                          new ParticipantModel { Id = model.Id };
        participant.Name = model.Name;
        participant.Type = model.Type;
        participant.VkId = model.VkId;
        participant.UserId = model.UserId;
        participant.ParentParticipantId = model.ParentParticipantId;
        participant.Notes = model.Notes;
        participant.Vip = model.Vip;
        return participant;
    }
}