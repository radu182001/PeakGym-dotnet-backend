using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class CommentDTO
{

    public Guid Id { get; set; }
    public UserDTO User { get; set; } = default!;

    public Guid TrainingPlanId { get; set; }

    public string Content { get; set; } = default!;

    public DateTime Timestamp { get; set; } = default!;

    public DateTime UpdatedAt { get; set; } = default!;

}
