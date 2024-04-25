using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Net;
using System.Numerics;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class CommentService : ICommentService
{

    private readonly IRepository<WebAppDatabaseContext> _repository;

    public CommentService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<CommentDTO>> GetComment(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new CommentProjectionSpec(id), cancellationToken);

        return result != null ?
            ServiceResponse<CommentDTO>.ForSuccess(result) :
            ServiceResponse<CommentDTO>.FromError(CommonErrors.CommentNotFound);
    }

    public async Task<ServiceResponse<PagedResponse<CommentDTO>>> GetCommentsForTrainingPlan(PaginationQueryParams pagination, Guid trainingPlanId, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new CommentProjectionSpec(trainingPlanId, 0), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<CommentDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> Add(CommentAddDTO comment, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {

        await _repository.AddAsync(new Comment
        {
            UserId = requestingUser.Id,
            TrainingPlanId = comment.TrainingPlanId,
            Content = comment.Content,
            Timestamp = DateTime.UtcNow,
        }, cancellationToken); // A new entity is created and persisted in the database.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> Update(CommentUpdateDTO comment, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {

        var entity = await _repository.GetAsync(new CommentSpec(comment.Id), cancellationToken);

        if (entity.UserId != requestingUser.Id)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the owner can update this comment!", ErrorCodes.CannotUpdate));
        }

        if (entity != null) // Verify if the user is not found, you cannot update an non-existing entity.
        {
            entity.Content = comment.Content ?? entity.Content;

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> Delete(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {

        var result = await _repository.GetAsync(new CommentProjectionSpec(id), cancellationToken);
        if (result == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Comment doesn't exist!", ErrorCodes.EntityNotFound));
        }

        if (result.User.Id != requestingUser.Id)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the owner can delete this comment!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<Comment>(id, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }

}
