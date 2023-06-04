using Overoom.Infrastructure.Storage.Mappers.Abstractions;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class ReportLogModelMapper : IModelMapperUnit<ReportLogModel, ReportLog>
{
    private readonly ApplicationDbContext _context;

    public ReportLogModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<ReportLogModel> MapAsync(ReportLog model)
    {
        var reportLog = await _context.ReportLogs.FirstOrDefaultAsync(x => x.Id == model.Id) ??
                        new ReportLogModel { Id = model.Id };
        reportLog.ReportId = model.ReportId;
        reportLog.UserId = model.UserId;
        reportLog.Success = model.Success;
        reportLog.CreatedAt = model.CreatedAt;
        reportLog.FinishedAt = model.FinishedAt;
        reportLog.AdditionalInfo = model.AdditionalInfo;
        reportLog.Type = model.Type;
        return reportLog;
    }
}