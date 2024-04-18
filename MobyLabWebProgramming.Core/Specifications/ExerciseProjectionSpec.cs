using System.Linq.Expressions;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class ExerciseProjectionSpec : BaseSpec<ExerciseProjectionSpec, Exercise, ExerciseDTO>
{
    /// <summary>
    /// This is the projection/mapping expression to be used by the base class to get UserDTO object from the database.
    /// </summary>
    protected override Expression<Func<Exercise, ExerciseDTO>> Spec => e => new()
    {
        Id = e.Id,
        Name = e.Name,
        MuscleGroup = e.MuscleGroup,
        SetsNo = e.SetsNo,
        Trainer = new()
        {
            Id = e.Trainer.Id,
            Name = e.Trainer.Name,
        }
    };

    public ExerciseProjectionSpec(bool orderByCreatedAt = true) : base(orderByCreatedAt)
    {
    }

    public ExerciseProjectionSpec(Guid id) : base(id)
    {
    }

    public ExerciseProjectionSpec(string? search)
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

    public ExerciseProjectionSpec(string? search, Guid? id, bool isTrainerId)
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

        if (isTrainerId)
        {
            Query.Where(e => e.Trainer.Id == id);
        }
        else
        {
            Query
                .Where(e => e.TrainingPlanExercises.Any(te => te.TrainingPlanId == id));
        }

        

        

        Query.Where(e => EF.Functions.ILike(e.Name, searchExpr));
    }

}
