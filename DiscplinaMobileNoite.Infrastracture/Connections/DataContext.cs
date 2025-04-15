using DiscplinaMobileNoite.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DiscplinaMobileNoite.Infrastracture.Connections
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PointEntity> Points { get; set; }
        public DbSet<JustificationEntity> Justifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            DataModelConfiguration.ConfigureModels(modelBuilder);
        }
    }
}