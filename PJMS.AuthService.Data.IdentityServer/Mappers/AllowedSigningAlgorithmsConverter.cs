using AutoMapper;

namespace PJMS.AuthService.Data.IdentityServer.Mappers;

internal class AllowedSigningAlgorithmsConverter :
    IValueConverter<ICollection<string>, string>,
    IValueConverter<string, ICollection<string>>
{
    public static readonly AllowedSigningAlgorithmsConverter Converter = new();

    public string Convert(ICollection<string> sourceMember, ResolutionContext context)
    {
        if (sourceMember == null || sourceMember.Count == 0)
        {
            return null;
        }

        return sourceMember.Aggregate((x, y) => $"{x},{y}");
    }

    public ICollection<string> Convert(string sourceMember, ResolutionContext context)
    {
        var list = new HashSet<string>();
        if (string.IsNullOrWhiteSpace(sourceMember)) return list;
        sourceMember = sourceMember.Trim();
        foreach (var item in sourceMember.Split(',', StringSplitOptions.RemoveEmptyEntries).Distinct())
        {
            list.Add(item);
        }

        return list;
    }
}