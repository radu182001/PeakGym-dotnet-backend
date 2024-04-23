using MobyLabWebProgramming.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobyLabWebProgramming.Core.Entities;

public class Comment : BaseEntity
{

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid TrainingPlanId { get; set; }

    public TrainingPlan TrainingPlan { get; set; } = default!;

    public string Content { get; set; } = default!;

    public DateTime Timestamp { get; set; } = default!;

}
