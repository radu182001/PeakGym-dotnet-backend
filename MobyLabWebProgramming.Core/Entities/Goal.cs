using MobyLabWebProgramming.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobyLabWebProgramming.Core.Entities;

public class Goal : BaseEntity
{

    public Guid ClientId { get; set; }
    [ForeignKey("ClientId")]
    public User Client { get; set; } = default!;

    public string Name { get; set; } = default!;
    public float TargetValue { get; set; } = default!;
    public float CurrentValue { get; set; } = default!;

}
