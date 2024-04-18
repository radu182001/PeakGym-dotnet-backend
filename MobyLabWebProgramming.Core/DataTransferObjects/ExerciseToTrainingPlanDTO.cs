using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class ExerciseToTrainingPlanDTO
{

    public Guid TrainingPlanId { get; set; }
    public Guid ExerciseId { get; set; }
    public int Weekday { get; set; } = default!;

}
