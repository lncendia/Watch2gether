using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class CommentReportModelMapper : IModelMapperUnit<CommentReportModel, CommentReport>
{
    private readonly ApplicationDbContext _context;

    public CommentReportModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<CommentReportModel> MapAsync(CommentReport model)
    {
        var commentReport = await _context.CommentReports.FirstOrDefaultAsync(x => x.Id == model.Id);
        if (commentReport != null)
        {
            await _context.Entry(commentReport).Collection(x => x.Publications).LoadAsync();
            await _context.Entry(commentReport).Collection(x => x.ReportElementsList).LoadAsync();
            await _context.Entry(commentReport).Collection(x => x.LinkedUsers).LoadAsync();
        }
        else
        {
            commentReport = new CommentReportModel();
        }


        if (!commentReport.ReportElementsList.Any())
        {
            commentReport.ReportElementsList.AddRange(model.Elements.Select(Create));
        }
        else
        {
            var modelElements = model.Elements.OrderBy(x => x.Id).ToList();
            var reportElements = commentReport.ReportElementsList.OrderBy(x => x.Id).ToList();
            for (var i = 0; i < modelElements.Count; i++) Map(modelElements[i], reportElements[i]);
        }

        if (!commentReport.LinkedUsers.Any())
        {
            var users = await _context.Users.Where(x => model.LinkedUsers.Any(y => y == x.Id)).ToListAsync();
            commentReport.LinkedUsers = users;
        }

        RoomModelInitializer.InitPublicationReportModel(commentReport, model);

        return commentReport;
    }


    private static CommentReportElementModel Create(CommentReportElement element)
    {
        var model = new CommentReportElementModel
        {
            EntityId = element.Id, Name = element.Name, IsAccepted = element.IsAccepted,
            ParticipantId = element.ParticipantId, VkId = element.VkId, LikeChatName = element.LikeChatName,
            Note = element.Note, OwnerId = element.Parent?.Id, Vip = element.Vip,
            Comments = GetCommentsRawString(element.Comments)
        };

        return model;
    }

    private static string GetCommentsRawString(IEnumerable<CommentInfo> comments) => string.Join(';',
        comments.Select(comment =>
            $"{comment.PublicationId}:{(comment.IsConfirmed ? '1' : '0')}:{comment.Text ?? string.Empty}"));

    private static void Map(CommentReportElement element, CommentReportElementModel model)
    {
        model.Comments = GetCommentsRawString(element.Comments);
        model.IsAccepted = element.IsAccepted;
    }
}