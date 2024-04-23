using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to update a user, the properties besides the id are nullable to indicate that they may not be updated if they are null.
/// </summary>
public record GoalUpdateDTO(Guid Id, string? Name = default, float? TargetValue = default, float? CurrentValue = default);

