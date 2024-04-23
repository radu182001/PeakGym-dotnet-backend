using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class CommentAddDTO
{

    public Guid TrainingPlanId { get; set; }

    public string Content { get; set; } = default!;

}
