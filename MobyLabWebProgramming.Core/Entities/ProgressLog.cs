using MobyLabWebProgramming.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace MobyLabWebProgramming.Core.Entities;

public class ProgressLog : BaseEntity
{

    public Guid ClientId { get; set; }
    [ForeignKey("ClientId")]
    public User Client { get; set; } = default!;

    public Guid TrainingPlanId { get; set; }
    public TrainingPlan TrainingPlan { get; set; } = default!;

    public DateTime Date { get; set; } = default!;

    public int Weekday { get; set; } = default!;

}
