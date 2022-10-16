using Watch2gether.Extensions;
using Watch2gether.WEB.Hubs;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();
builder.Services.AddAuthenticationServices();
builder.Services.AddInfrastructureServices(builder.Environment.WebRootPath);
builder.Services.AddPersistenceServices(connectionString);

builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();


var app = builder.Build();
//await app.Services.CreateScope().ServiceProvider.GetService<IRoomService>()!.RemoveAllRooms();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(x =>
{
    x.MapHub<FilmRoomHub>("/filmRoom");
    x.MapHub<YoutubeRoomHub>("/youtubeRoom");
});
app.Run();