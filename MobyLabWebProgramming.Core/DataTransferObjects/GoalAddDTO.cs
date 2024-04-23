using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class GoalAddDTO
{

    public string Name { get; set; } = default!;
    public float TargetValue { get; set; } = default!;
    public float CurrentValue { get; set; } = default!;

}
