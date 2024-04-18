using MobyLabWebProgramming.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class ExerciseDTO
{

    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public MuscleGroupEnum MuscleGroup { get; set; } = default!;
    public int SetsNo { get; set; } = default!;

    public UserDTO Trainer { get; set; } = default!;

}
