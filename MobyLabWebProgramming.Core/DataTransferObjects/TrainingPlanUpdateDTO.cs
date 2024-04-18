using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public record TrainingPlanUpdateDTO(Guid Id, string? Name = default, string? Description = default, int? DaysPerWeek = default);
