using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class TrainingPlanExerciseConfiguration : IEntityTypeConfiguration<TrainingPlanExercise>
{

    public void Configure(EntityTypeBuilder<TrainingPlanExercise> builder)
    {
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Weekday)
            .IsRequired();

        builder.HasOne(e => e.TrainingPlan)
            .WithMany(e => e.TrainingPlanExercises)
            .HasForeignKey(e => e.TrainingPlanId)
            .HasPrincipalKey(e => e.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Exercise)
            .WithMany(e => e.TrainingPlanExercises)
            .HasForeignKey(e => e.ExerciseId)
            .HasPrincipalKey(e => e.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
