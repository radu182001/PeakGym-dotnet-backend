using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class ProgressLogConfiguration
{

    public void Configure(EntityTypeBuilder<ProgressLog> builder)
    {
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Date)
            .HasDefaultValue(DateTime.Now)
            .IsRequired();
        builder.Property(e => e.Weekday)
            .IsRequired();
        builder.HasIndex(p => new { p.Date, p.Weekday })
            .IsUnique();

        builder.HasOne(e => e.Client)
            .WithMany(e => e.ProgressLogs)
            .HasForeignKey(e => e.ClientId)
            .HasPrincipalKey(e => e.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.TrainingPlan)
            .WithMany(e => e.ProgressLogs)
            .HasForeignKey(e => e.TrainingPlanId)
            .HasPrincipalKey(e => e.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

    }
}
