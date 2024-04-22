using System.Linq.Expressions;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class ProgressLogProjectionSpec : BaseSpec<ProgressLogProjectionSpec, ProgressLog, ProgressLogDTO>
{
    /// <summary>
    /// This is the projection/mapping expression to be used by the base class to get UserDTO object from the database.
    /// </summary>
    protected override Expression<Func<ProgressLog, ProgressLogDTO>> Spec => e => new()
    {
        Id = e.Id,
        ClientId = e.ClientId,
        TrainingPlanId = e.TrainingPlanId,
        Date = e.Date,
        Weekday = e.Weekday
    };

    public ProgressLogProjectionSpec(bool orderByCreatedAt = true) : base(orderByCreatedAt)
    {
    }

    public ProgressLogProjectionSpec(Guid id) : base(id)
    {
    }

    public ProgressLogProjectionSpec(string? search)
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

    public ProgressLogProjectionSpec(Guid clientId, int filter) // 0 for filter is all time, 1 is last year, 2 is last month, 3 last week
    {

        Query.Where(e => e.ClientId == clientId);

        if (filter == 1)
        {
            // Filter by last year
            var lastYearDate = DateTime.Now.AddYears(-1);
            Query.Where(e => e.Date >= lastYearDate);
        }
        else if (filter == 2)
        {
            // Filter by last month
            var lastMonthDate = DateTime.Now.AddMonths(-1);
            Query.Where(e => e.Date >= lastMonthDate);
        }
        else if (filter == 3)
        {
            // Filter by last week
            var lastWeekDate = DateTime.Now.AddDays(-7);
            Query.Where(e => e.Date >= lastWeekDate);
        }

        //Query.Where(e => EF.Functions.ILike(e.Name, searchExpr)); // This is an example on who database specific expressions can be used via C# expressions.
        // Note that this will be translated to the database something like "where user.Name ilike '%str%'".
    }

}

