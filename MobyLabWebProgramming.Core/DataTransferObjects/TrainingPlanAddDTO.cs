using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class TrainingPlanAddDTO
{

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public int DaysPerWeek { get; set; } = default!;

}
