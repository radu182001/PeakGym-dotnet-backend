using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class GoalDTO
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }

    public string Name { get; set; } = default!;
    public float TargetValue { get; set; } = default!;
    public float CurrentValue { get; set; } = default!;

}
