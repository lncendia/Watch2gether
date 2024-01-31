using AuthService.Infrastructure.Storage;
using AuthService.Start.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configuration"))
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("email.json", optional: false, reloadOnChange: true)
    .AddJsonFile("oauth.json", optional: false, reloadOnChange: true)
    .AddJsonFile("jwt.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);


builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddEmailServices(builder.Configuration);
builder.Services.AddMediatorServices();
builder.Services.AddOauthServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration, builder.Environment.WebRootPath);
builder.Services.AddControllersWithViews().AddViewLocalization().AddRazorRuntimeCompilation();
builder.Services.AddLocalizationServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    await context.Database.MigrateAsync();
}

app.UseExceptionHandler("/Home/Error");
app.UseHsts();

// Добавляет RequestLocalizationMiddleware для автоматической установки
// сведений о культуре для запросов на основе информации, предоставленной клиентом.
app.UseRequestLocalization();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}");
app.Run();