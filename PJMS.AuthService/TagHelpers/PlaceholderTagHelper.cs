using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PJMS.AuthService.TagHelpers;

/// <summary>
/// Класс, реализующий тег-помощник для добавления атрибута placeholder.
/// </summary>
[HtmlTargetElement("span", Attributes = PlaceholderAttributeName, TagStructure = TagStructure.WithoutEndTag)]
public class PlaceholderTagHelper : TagHelper
{
    /// <summary>
    /// Имя атрибута placeholder.
    /// </summary>
    private const string PlaceholderAttributeName = "asp-placeholder-for";

    /// <summary>
    /// Выражение, которое будет вычислено относительно текущей модели.
    /// </summary>
    [HtmlAttributeName(PlaceholderAttributeName)]
    public ModelExpression Placeholder { get; set; } = null!;
    
    /// <summary> 
    /// Метод для обработки элемента HTML в Tag Helper. 
    /// </summary> 
    /// <param name="context">Контекст Tag Helper.</param> 
    /// <param name="output">Выходные данные Tag Helper.</param> 
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Вызываем базовую реализацию
        base.Process(context, output);

        // Получаем значение заполнителя из Placeholder.ModelExplorer
        var placeholder = GetPlaceholder(Placeholder.ModelExplorer);

        // Если атрибут data-placeholder отсутствует, добавляем его
        if (!output.Attributes.TryGetAttribute("data-placeholder", out _))
        {
            output.Attributes.Add(new TagHelperAttribute("data-placeholder", placeholder));
        }
    }

    /// <summary>
    /// Получает заполнитель для указанного ModelExplorer.
    /// </summary>
    /// <param name="modelExplorer">Модельный исследователь для получения информации о модели.</param>
    /// <returns>Заполнитель для модели.</returns>
    private static string GetPlaceholder(ModelExplorer modelExplorer)
    {
        // Получаем значение заполнителя из ModelExplorer.Metadata.Placeholder
        var placeholder = modelExplorer.Metadata.Placeholder;

        // Если значение заполнителя пустое или состоит только из пробелов, получаем отображаемое имя модели
        if (string.IsNullOrWhiteSpace(placeholder))
        {
            placeholder = modelExplorer.Metadata.GetDisplayName();
        }

        return placeholder;
    }
}