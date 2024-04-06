using Domain.Enities;
using Infrastrucure.DAL.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastrucure
{
    public class AplicationContext:DbContext
    {
        public DbSet<User>Persons { get; set; }
        public DbSet<Ads>Ads { get; set; }


        private readonly IConfiguration _configuration;

        public AplicationContext(DbContextOptions<AplicationContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AdsConfiguration());
        }

    }
}