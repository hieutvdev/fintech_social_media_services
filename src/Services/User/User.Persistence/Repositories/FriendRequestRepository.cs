using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using BuildingBlocks.Repository.Cache;
using BuildingBlocks.Repository.EntityFrameworkBase.MultipleContext;
using BuildingBlocks.Security;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using ShredKernel.BaseClasses;
using ShredKernel.Specifications;
using User.Application.DTOs.Request.FriendRequest;
using User.Application.DTOs.Response.FriendRequest;
using User.Application.Repositories;
using User.Domain.Models;
using User.Domain.ValuesObjects;
using User.Persistence.Data;

namespace User.Persistence.Repositories;

public class FriendRequestRepository(IRepositoryBaseService<ApplicationDbContext> repository, IDistributedCacheService cacheService, IAuthorizeExtension authorizeExtension) : IFriendRequestRepository
{
    public async Task<bool> CreateAsync(CreateFriendRequestReqDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var friendRequestId = FriendRequestId.Of(Guid.NewGuid());
            var userId = authorizeExtension.GetUserFromClaimToken().Id;
            var friendRequest = FriendRequest.Create(friendRequestId, userId, payload.ReceiverId);
            await repository.AddAsync(friendRequest, cancellationToken);
            var isSuccessful = await repository.SaveChangesAsync(cancellationToken) > 0;
            return isSuccessful;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> UpdateAsync(UpdateStatusFriendRequestReqDto payload, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(DeleteFriendRequestReqDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var friendRequestId = FriendRequestId.Of(Guid.Parse(payload.Id));
            var userId = authorizeExtension.GetUserFromClaimToken().Id;
            
            var isSuccessful = await repository.ExecuteDeleteAsync<FriendRequest>(r => r.Id == friendRequestId && r.SenderId == userId, cancellationToken);
            return isSuccessful;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<FriendRequestByUserResDto>> GetListBySenderAsync(PaginationRequest query, CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = authorizeExtension.GetUserFromClaimToken().Id;
            var friendRequests = await repository.Table<FriendRequest>().Select(r => new
            {
                Id = r.Id,
                SenderId = r.SenderId,
                ReceiverId = r.ReceiverId,
                SendAt = r.SendAt,
            }).Where(r => r.SenderId == userId).OrderBy(r => r.SendAt).Skip((query.PageIndex - 1)*query.PageSize).Take(query.PageSize).ToListAsync(cancellationToken);


            var response =
                new PaginatedResult<FriendRequestByUserResDto>(query.PageIndex, query.PageSize, 0,
                    friendRequests.Select(r => new FriendRequestByUserResDto(r.Id.Value.ToString(), r.ReceiverId, r.SendAt)).ToList());
            

            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<FriendRequestByUserResDto>> GetListByReceiverIdAsync(PaginationRequest query, CancellationToken cancellationToken = default)
    {
        var userId = authorizeExtension.GetUserFromClaimToken().Id;
        var friendRequests = await repository.Table<FriendRequest>().Select(r => new
        {
            Id = r.Id,
            SenderId = r.SenderId,
            ReceiverId = r.ReceiverId,
            SendAt = r.SendAt,
        }).Where(r => r.ReceiverId == userId).OrderBy(r => r.SendAt).Skip((query.PageIndex - 1)*query.PageSize).Take(query.PageSize).ToListAsync(cancellationToken);


        var response =
            new PaginatedResult<FriendRequestByUserResDto>(query.PageIndex, query.PageSize, 0,
                friendRequests.Select(r => new FriendRequestByUserResDto(r.Id.Value.ToString(), r.ReceiverId, r.SendAt)).ToList());
            

        return response;
    }

    public async Task<PaginatedResult<FriendRequestByUserLoginResDto>> GetListByUserLoginAsync(PaginationRequest query, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}