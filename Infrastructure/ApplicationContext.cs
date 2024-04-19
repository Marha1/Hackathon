using Domain.Enities;
using Infrastructure.DAL.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

/// <summary>
///     Представляет контекст базы данных для приложения.
/// </summary>
public class AplicationContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AplicationContext(DbContextOptions<AplicationContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    ///     Получает или задает набор пользователей в базе данных.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    ///     Получает или задает набор объявлений в базе данных.
    /// </summary>
    public DbSet<Ads> Ads { get; set; }

    /// <summary>
    ///     Настраивает модель для базы данных.
    /// </summary>
    /// <param name="modelBuilder">Экземпляр построителя модели.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new AdsConfiguration());
    }
}