using System.Globalization;
using Films.Start.Extensions;
using Films.Start.HostedServices;

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru-RU")
{
    NumberFormat = new NumberFormatInfo
    {
        NumberDecimalSeparator = "."
    }
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddApplicationMappers();
builder.Services.AddControllerMappers();
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment.WebRootPath);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddEventHandlers();

builder.Services.AddHostedService<FilmLoadHostedService>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");
app.UseHsts();

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();