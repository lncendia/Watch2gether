using Microsoft.EntityFrameworkCore;
using PJMS.AuthService.Data.IdentityServer.Entities;

namespace PJMS.AuthService.Data.IdentityServer.DbContexts;

/// <summary> 
/// Контекст данных для операционных данных IdentityServer. 
/// </summary> 
/// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" /> 
public class PersistedGrantDbContext(DbContextOptions<PersistedGrantDbContext> options) : DbContext(options)
{
    /// <summary> 
    /// Получает или задает сохраненные гранты. 
    /// </summary> 
    /// <value> 
    /// Сохраненные гранты. 
    /// </value> 
    public DbSet<PersistedGrant> PersistedGrants { get; set; }

    /// <summary> 
    /// Получает или задает коды устройств. 
    /// </summary> 
    /// <value> 
    /// Коды устройств. 
    /// </value> 
    public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

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
        modelBuilder.ConfigurePersistedGrantContext();
        base.OnModelCreating(modelBuilder);
    }
}