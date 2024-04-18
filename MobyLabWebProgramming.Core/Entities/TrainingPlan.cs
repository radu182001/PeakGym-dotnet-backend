using MobyLabWebProgramming.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobyLabWebProgramming.Core.Entities;

public class TrainingPlan : BaseEntity
{

    public string Name { get; set; } = default!;
    public int DaysPerWeek { get; set; } = default!;

    public string Description { get; set; } = default!;

    public Guid TrainerId { get; set; }
    [ForeignKey("TrainerId")]
    public User Trainer { get; set; } = default!;

    public ICollection<User> Users { get; set; } = default!;

    // Merge table for many to many relationship with Exercises
    public ICollection<TrainingPlanExercise> TrainingPlanExercises { get; set; } = default!;

}
