using MobyLabWebProgramming.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobyLabWebProgramming.Core.Entities;

public class Exercise : BaseEntity
{

    public string Name { get; set; } = default!;
    public MuscleGroupEnum MuscleGroup { get; set; } = default!;
    public int SetsNo { get; set; } = default!;

    public Guid TrainerId { get; set; }
    [ForeignKey("TrainerId")]
    public User Trainer { get; set; } = default!;

    // Merge table for many to many relationship with Exercises
    public ICollection<TrainingPlanExercise> TrainingPlanExercises { get; set; } = default!;
}
