

using MobyLabWebProgramming.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class ProgressLogAddDTO
{

    public int Weekday { get; set; } = default!;

}
