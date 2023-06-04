using System.Reflection;
using System.Runtime.Serialization;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Mappers.StaticMethods;

namespace Overoom.Infrastructure.Storage.Mappers.AggregateMappers;

internal class LikeReportMapper : IAggregateMapperUnit<LikeReport, LikeReportModel>
{
    private static readonly Type LikeReportType = typeof(LikeReport);
    private static readonly Type LikeReportElementType = typeof(LikeReportElement);
    private static readonly Type LikeInfoElementType = typeof(LikeInfo);

    private static readonly FieldInfo ElementLikes =
        LikeReportElementType.GetField("_likes", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public LikeReport Map(LikeReportModel model)
    {
        var report = (LikeReport)FormatterServices.GetUninitializedObject(LikeReportType);
        var elementsGrouping = model.ReportElementsList.GroupBy(x => x.OwnerId).ToList();
        var elements = new List<LikeReportElement>();

        if (elementsGrouping.Any())
        {
            foreach (var group in elementsGrouping.First(x => x.Key == null))
            {
                var parent = GetLikeElement(group, null);
                elements.Add(parent);
                var children = elementsGrouping.FirstOrDefault(x => x.Key == group.EntityId);
                if (children == null) continue;
                elements.AddRange(children.Select(x => GetLikeElement(x, parent)));
            }
        }

        RoomInitializer.InitPublicationReport(report, elements, model);
        return report;
    }

    private static LikeReportElement GetLikeElement(LikeReportElementModel model, LikeReportElement? owner)
    {
        object?[] args = { model.Name, model.LikeChatName, model.VkId, model.ParticipantId, model.Vip, model.Note, owner, model.EntityId };
        var element = (LikeReportElement)LikeReportElementType.Assembly.CreateInstance(
            LikeReportElementType.FullName!, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args!,
            null, null)!;
        ElementLikes.SetValue(element, GetLikesInfo(model.Likes));
        if (model.IsAccepted) element.Accept();
        return element;
    }

    private static List<LikeInfo> GetLikesInfo(string rawString)
    {
        var data = rawString.Split(';');
        var list = new List<LikeInfo>();
        foreach (var value in data)
        {
            var likeInfo = value.Split(':');
            var id = int.Parse(likeInfo[0]);
            var like = int.Parse(likeInfo[1]) == 1;
            object?[] args = { id, like };
            var element = (LikeInfo)LikeInfoElementType.Assembly.CreateInstance(
                LikeInfoElementType.FullName!, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args!,
                null, null)!;
            list.Add(element);
        }

        return list;
    }
}