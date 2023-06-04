using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class LikeReportModelMapper : IModelMapperUnit<LikeReportModel, LikeReport>
{
    private readonly ApplicationDbContext _context;

    public LikeReportModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<LikeReportModel> MapAsync(LikeReport model)
    {
        var likeReport = await _context.LikeReports.FirstOrDefaultAsync(x => x.Id == model.Id);
        if (likeReport != null)
        {
            await _context.Entry(likeReport).Collection(x => x.Publications).LoadAsync();
            await _context.Entry(likeReport).Collection(x => x.ReportElementsList).LoadAsync();
            await _context.Entry(likeReport).Collection(x => x.LinkedUsers).LoadAsync();
        }
        else
        {
            likeReport = new LikeReportModel();
        }


        if (!likeReport.ReportElementsList.Any())
        {
            likeReport.ReportElementsList.AddRange(model.Elements.Select(Create));
        }
        else
        {
            var modelElements = model.Elements.OrderBy(x => x.Id).ToList();
            var reportElements = likeReport.ReportElementsList.OrderBy(x => x.Id).ToList();
            for (var i = 0; i < modelElements.Count; i++) Map(modelElements[i], reportElements[i]);
        }

        if (!likeReport.LinkedUsers.Any())
        {
            var users = await _context.Users.Where(x => model.LinkedUsers.Any(y => y == x.Id)).ToListAsync();
            likeReport.LinkedUsers = users;
        }

        RoomModelInitializer.InitPublicationReportModel(likeReport, model);

        return likeReport;
    }


    private static LikeReportElementModel Create(LikeReportElement element)
    {
        var model = new LikeReportElementModel
        {
            EntityId = element.Id, Name = element.Name, IsAccepted = element.IsAccepted,
            ParticipantId = element.ParticipantId, VkId = element.VkId, LikeChatName = element.LikeChatName,
            Note = element.Note, OwnerId = element.Parent?.Id, Vip = element.Vip,
            Likes = GetLikesRawString(element.Likes)
        };

        return model;
    }

    private static string GetLikesRawString(IEnumerable<LikeInfo> likes) => string.Join(';',
        likes.Select(like => $"{like.PublicationId}:{(like.IsConfirmed ? '1' : '0')}"));

    private static void Map(LikeReportElement element, LikeReportElementModel model)
    {
        model.Likes = GetLikesRawString(element.Likes);
        model.IsAccepted = element.IsAccepted;
    }
}