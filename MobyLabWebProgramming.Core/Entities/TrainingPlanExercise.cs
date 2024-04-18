namespace MobyLabWebProgramming.Core.Entities;

public class TrainingPlanExercise : BaseEntity
{

    public Guid TrainingPlanId { get; set; }
    public TrainingPlan TrainingPlan { get; set; } = default!;

    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = default!;
    public int Weekday { get; set; } = default;

}

