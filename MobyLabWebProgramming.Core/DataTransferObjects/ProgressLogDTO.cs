

using MobyLabWebProgramming.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class ProgressLogDTO
{

    public Guid Id { get; set; }

    public Guid ClientId { get; set; }
    public Guid TrainingPlanId { get; set; }

    public DateTime Date { get; set; } = default!;

    public int Weekday { get; set; } = default!;

}
