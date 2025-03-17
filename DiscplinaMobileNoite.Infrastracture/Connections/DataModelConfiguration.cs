using DiscplinaMobileNoite.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DiscplinaMobileNoite.Infrastracture.Connections
{
    public static class DataModelConfiguration
    {
        public static void ConfigureModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Cpf).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.JobTitle).HasMaxLength(50);
            });
        }
    }
}