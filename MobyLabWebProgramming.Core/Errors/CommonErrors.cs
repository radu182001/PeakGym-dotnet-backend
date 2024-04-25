using System.Net;

namespace MobyLabWebProgramming.Core.Errors;

/// <summary>
/// Common error messages that may be reused in various places in the code.
/// </summary>
public static class CommonErrors
{
    public static ErrorMessage UserNotFound => new(HttpStatusCode.NotFound, "User doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage TrainingPlanNotFound => new(HttpStatusCode.NotFound, "Training plan doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage ExerciseNotFound => new(HttpStatusCode.NotFound, "Exercise doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage CommentNotFound => new(HttpStatusCode.NotFound, "Comment doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage GoalNotFound => new(HttpStatusCode.NotFound, "Goal doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage ProgressLogNotFound => new(HttpStatusCode.NotFound, "Progress log doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage FileNotFound => new(HttpStatusCode.NotFound, "File not found on disk!", ErrorCodes.PhysicalFileNotFound);
    public static ErrorMessage TechnicalSupport => new(HttpStatusCode.InternalServerError, "An unknown error occurred, contact the technical support!", ErrorCodes.TechnicalError);
}
