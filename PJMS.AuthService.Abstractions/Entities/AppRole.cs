using Microsoft.AspNetCore.Identity;

namespace PJMS.AuthService.Abstractions.Entities;

/// <summary>
/// Класс, представляющий роль приложения.
/// </summary>
public class AppRole : IdentityRole<Guid>
{
    /// <summary>
    /// Описание роли.
    /// </summary>
    public string? Description { get; set; }
}