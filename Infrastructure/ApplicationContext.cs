using Domain.Enities;
using Infrastructure.DAL.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class AplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Ads> Ads { get; set; }

        private readonly IConfiguration _configuration;

        public AplicationContext(DbContextOptions<AplicationContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AdsConfiguration());
        }
    }
}