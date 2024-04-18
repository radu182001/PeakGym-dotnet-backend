using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.Entities;

public class Exercise : BaseEntity
{

    public string Name { get; set; } = default!;
    public MuscleGroupEnum MuscleGroup { get; set; } = default!;
    public int SetsNo { get; set; } = default!;


    //public ICollection<TrainingPlan> TrainingPlans { get; set; } = default!;
}
