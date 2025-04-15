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

                entity.Property(u => u.FullName).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.Workload).IsRequired();
                entity.Property(u => u.PhoneNumber).IsRequired();
                entity.Property(u => u.CreatedAt).IsRequired();

                entity.HasMany(u => u.PointsEntity)
                      .WithOne(p => p.UserEntity)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PointEntity>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.UserId).IsRequired();
                entity.Property(p => p.Date).IsRequired();
                entity.Property(p => p.MorningEntry).IsRequired(false);
                entity.Property(p => p.MorningExit).IsRequired(false);
                entity.Property(p => p.AfternoonEntry).IsRequired(false);
                entity.Property(p => p.AfternoonExit).IsRequired(false);
                entity.Property(p => p.Status).IsRequired();
                entity.Property(p => p.CreatedAt).IsRequired();

                entity.HasOne(p => p.UserEntity)
                      .WithMany(u => u.PointsEntity)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.JustificationEntity)
                      .WithOne(j => j.PointsEntity)
                      .HasForeignKey(j => j.PointId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<JustificationEntity>(entity =>
            {
                entity.HasKey(j => j.Id);

                entity.Property(j => j.PointId).IsRequired();
                entity.Property(j => j.Reason).IsRequired().HasMaxLength(255);
                entity.Property(j => j.Status).IsRequired();
                entity.Property(j => j.CreatedAt).IsRequired();

                entity.HasOne(j => j.PointsEntity)
                      .WithMany(p => p.JustificationEntity)
                      .HasForeignKey(j => j.PointId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}