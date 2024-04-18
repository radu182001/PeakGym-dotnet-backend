using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{

    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(e => e.MuscleGroup)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(e => e.SetsNo)
            .IsRequired();
        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();


        builder.HasOne(e => e.Trainer)
            .WithMany(e => e.Exercises)
            .HasForeignKey(e => e.TrainerId)
            .HasPrincipalKey(e => e.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

}
