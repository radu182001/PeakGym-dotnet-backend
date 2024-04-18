namespace MobyLabWebProgramming.Core.Entities;

class TrainingPlanExercise
{

    public Guid TrainingPlanId { get; set; }
    public TrainingPlan TrainingPlan { get; set; } = default!;

    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = default!;

}

