using Overoom.Infrastructure.Storage.Mappers.Abstractions;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class LinkModelMapper : IModelMapperUnit<LinkModel, Link>
{
    private readonly ApplicationDbContext _context;

    public LinkModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<LinkModel> MapAsync(Link model)
    {
        var link = await _context.Links.FirstOrDefaultAsync(x => x.Id == model.Id) ??
                   new LinkModel { Id = model.Id };
        link.User1Id = model.User1Id;
        link.User2Id = model.User2Id;
        link.IsAccepted = model.IsConfirmed;
        return link;
    }
}