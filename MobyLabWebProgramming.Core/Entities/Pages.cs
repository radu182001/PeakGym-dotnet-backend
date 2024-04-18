using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.Entities;
public class Pages : BaseEntity
{

    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;

}

