using Microsoft.EntityFrameworkCore;
using PJMS.AuthService.Data.IdentityServer.Entities;

namespace PJMS.AuthService.Data.IdentityServer.DbContexts;


/// <summary> 
/// Контекст данных для конфигурационных данных IdentityServer. 
/// </summary> 
/// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" /> 
public class ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : DbContext(options)
{
    /// <summary> 
    /// Получает или задает клиентов. 
    /// </summary> 
    /// <value> 
    /// Клиенты. 
    /// </value> 
    public DbSet<Client> Clients { get; set; }

    /// <summary> 
    /// Получает или задает CORS-оригины для клиентов. 
    /// </summary> 
    /// <value> 
    /// CORS-оригины для клиентов. 
    /// </value> 
    public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }

    /// <summary> 
    /// Получает или задает идентичные ресурсы. 
    /// </summary> 
    /// <value> 
    /// Идентичные ресурсы. 
    /// </value> 
    public DbSet<IdentityResource> IdentityResources { get; set; }

    /// <summary> 
    /// Получает или задает API-ресурсы. 
    /// </summary> 
    /// <value> 
    /// API-ресурсы. 
    /// </value> 
    public DbSet<ApiResource> ApiResources { get; set; }

    /// <summary> 
    /// Получает или задает области API. 
    /// </summary> 
    /// <value> 
    /// Области API. 
    /// </value> 
    public DbSet<ApiScope> ApiScopes { get; set; }

    /// <summary> 
    /// Переопределите этот метод, чтобы дополнительно настроить модель, которая была обнаружена по соглашению из типов сущностей, 
    /// представленных в свойствах <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> в вашем производном контексте. Результирующая модель может быть кэширована 
    /// и использоваться повторно для последующих экземпляров вашего производного контекста. 
    /// </summary> 
    /// <param name="modelBuilder">Используемый для построения модели этого контекста строитель. Базы данных (и другие расширения) обычно 
    /// определяют методы расширения на этом объекте, которые позволяют настроить аспекты модели, специфичные 
    /// для определенной базы данных.</param> 
    /// <remarks> 
    /// Если модель явно установлена в параметрах для этого контекста (через <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />), 
    /// то этот метод не будет выполняться. 
    /// </remarks> 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureClientContext();
        modelBuilder.ConfigureResourcesContext();
        base.OnModelCreating(modelBuilder);
    }
}