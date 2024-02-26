using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PJMS.AuthService.Abstractions.Entities;
using PJMS.AuthService.Abstractions.Enums;
using PJMS.AuthService.Data.DbContexts;

namespace PJMS.AuthService.Data.DatabaseInitialization;

/// <summary>
/// Класс для инициализации начальных данных в базу данных
/// </summary>
public static class DatabaseInitializer
{
    /// <summary>
    /// Инициализация начальных данных в базу данных
    /// </summary>
    /// <param name="scopeServiceProvider">Определяет механизм для извлечения объекта службы,
    /// т. е. объекта, обеспечивающего настраиваемую поддержку для других объектов.</param>
    public static async Task InitAsync(IServiceProvider scopeServiceProvider)
    {
        // Получаем контекст базы данных
        var context = scopeServiceProvider.GetRequiredService<ApplicationDbContext>();

        //обновляем базу данных
        await context.Database.MigrateAsync();

        // Получаем экземпляр сервиса UserManager<AppUser> из провайдера служб scopeServiceProvider.
        var userManager = scopeServiceProvider.GetRequiredService<UserManager<AppUser>>();

        // Получаем экземпляр сервиса RoleManager<AppRole> из провайдера служб scopeServiceProvider.
        var roleManager = scopeServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        // Определение константных строковых переменных для имен администратора, пользователя и менеджера.
        const string adminEmail = "admin@gmail.com";

        //получаем текущую дату и время
        var now = DateTime.UtcNow;

        // Проверяем, существует ли роль "admin". Если нет, то создаем новую роль "admin".
        if (await roleManager.FindByNameAsync("admin") == null)
        {
            await roleManager.CreateAsync(new AppRole { Name = "admin", Description = "Administrator" });
        }

        // Проверяем, существует ли пользователь с именем adminName.
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            //создаем нового пользователя admin
            var admin = new AppUser
            {
                TimeRegistration = now,
                TimeLastAuth = now,
                Locale = Localization.Ru,
                Thumbnail = null,
                Email = adminEmail,
                UserName = adminEmail.Split('@')[0],
                EmailConfirmed = true
            };


            //ставим пароль
            var result = await userManager.CreateAsync(admin, "LuX995_WFB");

            //если успешно
            if (result.Succeeded)
            {
                //добавляем его в роль
                await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}