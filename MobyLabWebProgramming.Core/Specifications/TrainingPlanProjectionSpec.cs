using System.Linq.Expressions;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class TrainingPlanProjectionSpec : BaseSpec<TrainingPlanProjectionSpec, TrainingPlan, TrainingPlanDTO>
{
    /// <summary>
    /// This is the projection/mapping expression to be used by the base class to get UserDTO object from the database.
    /// </summary>
    protected override Expression<Func<TrainingPlan, TrainingPlanDTO>> Spec => e => new()
    {
        Id = e.Id,
        Name = e.Name,
        DaysPerWeek = e.DaysPerWeek,
        Description = e.Description,
        Trainer = new ()
        {
            Id = e.Trainer.Id,
            Name = e.Trainer.Name,
            Role = e.Trainer.Role,
        }
    };

    public TrainingPlanProjectionSpec(bool orderByCreatedAt = true) : base(orderByCreatedAt)
    {
    }

    public TrainingPlanProjectionSpec(Guid id) : base(id)
    {
    }

    public TrainingPlanProjectionSpec(string? search)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

        if (search == null)
        {
            return;
        }

        var searchExpr = $"%{search.Replace(" ", "%")}%";

        Query.Where(e => EF.Functions.ILike(e.Name, searchExpr)); // This is an example on who database specific expressions can be used via C# expressions.
                                                                  // Note that this will be translated to the database something like "where user.Name ilike '%str%'".
    }

    public TrainingPlanProjectionSpec(string? search, Guid? id)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

        if (search == null)
        {
            search = "";
        }

        if (id == null)
        {
            return;
        }

        var searchExpr = $"%{search.Replace(" ", "%")}%";

        Query.Where(e => e.Trainer.Id == id);

        Query.Where(e => EF.Functions.ILike(e.Name, searchExpr));
    }
}
