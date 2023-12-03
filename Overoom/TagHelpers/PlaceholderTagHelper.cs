using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Overoom.TagHelpers;

[HtmlTargetElement("span", Attributes = PlaceholderAttributeName, TagStructure = TagStructure.WithoutEndTag)]
public class PlaceholderTagHelper : TagHelper
{
    private const string PlaceholderAttributeName = "asp-placeholder-for";

    /// <summary>
    /// An expression to be evaluated against the current model.
    /// </summary>
    [HtmlAttributeName(PlaceholderAttributeName)]
    public ModelExpression Placeholder { get; set; } = null!;


    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);

        var placeholder = GetPlaceholder(Placeholder.ModelExplorer);

        if (!output.Attributes.TryGetAttribute("data-placeholder", out _))
        {
            output.Attributes.Add(new TagHelperAttribute("data-placeholder", placeholder));
        }
    }

    private static string GetPlaceholder(ModelExplorer modelExplorer)
    {
        var placeholder = modelExplorer.Metadata.Placeholder;

        if (string.IsNullOrWhiteSpace(placeholder))
        {
            placeholder = modelExplorer.Metadata.GetDisplayName();
        }

        return placeholder;
    }
}