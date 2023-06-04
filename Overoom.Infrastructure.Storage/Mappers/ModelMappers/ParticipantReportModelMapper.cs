using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class ParticipantReportModelMapper : IModelMapperUnit<ParticipantReportModel, ParticipantReport>
{
    private readonly ApplicationDbContext _context;

    public ParticipantReportModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<ParticipantReportModel> MapAsync(ParticipantReport model)
    {
        var participantReport = await _context.ParticipantReports.FirstOrDefaultAsync(x => x.Id == model.Id);

        if (participantReport != null)
        {
            await _context.Entry(participantReport).Collection(x => x.ReportElementsList).LoadAsync();
        }
        else
        {
            participantReport = new ParticipantReportModel();
        }

        if (!participantReport.ReportElementsList.Any())
        {
            participantReport.ReportElementsList.AddRange(
                model.Participants.Select(x => Create(x, participantReport)));
        }
        else
        {
            var allElements = model.Participants;
            var deleteElements = participantReport.ReportElementsList
                .ExceptBy(allElements.Select(x => x.Id), element => element.Id)
                .ToList();
            participantReport.ReportElementsList.RemoveAll(deleteElements.Contains);

            foreach (var x in model.Participants)
            {
                Map(x, participantReport.ReportElementsList.First(y => x.Id == y.Id));
            }
        }

        RoomModelInitializer.InitReportModel(participantReport, model);
        participantReport.VkId = model.VkId;
        return participantReport;
    }


    private static ParticipantReportElementModel Create(ParticipantReportElement element, ParticipantReportModel report)
    {
        var model = new ParticipantReportElementModel
        {
            EntityId = element.Id, Name = element.Name, Type = element.Type, NewName = element.NewName,
            VkId = element.VkId,
            OwnerId = element.Parent?.Id, Report = report, ParticipantType = element.ParticipantType
        };
        return model;
    }

    private static void Map(ParticipantReportElement element, ParticipantReportElementModel model)
    {
        model.Type = element.Type;
        model.NewName = element.NewName;
    }
}