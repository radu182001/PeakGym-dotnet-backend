using System.Linq.Expressions;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class GoalProjectionSpec : BaseSpec<GoalProjectionSpec, Goal, GoalDTO>
{
    /// <summary>
    /// This is the projection/mapping expression to be used by the base class to get UserDTO object from the database.
    /// </summary>
    protected override Expression<Func<Goal, GoalDTO>> Spec => e => new()
    {
        Id = e.Id,
        ClientId = e.ClientId,
        Name = e.Name,
        TargetValue = e.TargetValue,
        CurrentValue = e.CurrentValue,
    };

    public GoalProjectionSpec(bool orderByCreatedAt = true) : base(orderByCreatedAt)
    {
    }

    public GoalProjectionSpec(Guid id) : base(id)
    {
    }

    public GoalProjectionSpec(string? search)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

        if (search == null)
        {
            return;
        }

        var searchExpr = $"%{search.Replace(" ", "%")}%";

        //Query.Where(e => EF.Functions.ILike(e.Name, searchExpr)); // This is an example on who database specific expressions can be used via C# expressions.
        // Note that this will be translated to the database something like "where user.Name ilike '%str%'".
    }

    public GoalProjectionSpec(Guid id, int flag)
    {

        Query.Where(e => e.ClientId == id); // This is an example on who database specific expressions can be used via C# expressions.
        // Note that this will be translated to the database something like "where user.Name ilike '%str%'".
    }

}

