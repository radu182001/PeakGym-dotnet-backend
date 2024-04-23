using System.Linq.Expressions;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class CommentProjectionSpec : BaseSpec<CommentProjectionSpec, Comment, CommentDTO>
{
    /// <summary>
    /// This is the projection/mapping expression to be used by the base class to get UserDTO object from the database.
    /// </summary>
    protected override Expression<Func<Comment, CommentDTO>> Spec => e => new()
    {
        Id = e.Id,
        User = new()
        { 
            Id = e.User.Id,
            Name = e.User.Name,
            Role = e.User.Role,
        },
        TrainingPlanId = e.TrainingPlanId,
        Content = e.Content,
        Timestamp = e.Timestamp,
        UpdatedAt = e.UpdatedAt,
    };

    public CommentProjectionSpec(bool orderByCreatedAt = true) : base(orderByCreatedAt)
    {
    }

    public CommentProjectionSpec(Guid id) : base(id)
    {
    }

    public CommentProjectionSpec(string? search)
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

    public CommentProjectionSpec(Guid id, int flag)
    {

        Query.Where(e => e.TrainingPlanId == id); // This is an example on who database specific expressions can be used via C# expressions.
        // Note that this will be translated to the database something like "where user.Name ilike '%str%'".
    }

}

