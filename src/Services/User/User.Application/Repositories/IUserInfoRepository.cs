﻿using BuildingBlocks.Messaging.MessageModels.AuthService;
using User.Application.DTOs.Request.UserInfo;
using User.Application.DTOs.Response.UserInfo;
using User.Domain.Models;

namespace User.Application.Repositories;

public interface IUserInfoRepository
{
    Task<bool> CreateUserInfoAsync(CreateUserInfoReqDto payload, CancellationToken cancellationToken = default!);
    
    Task<bool> UpdateUserInfoAsync(UpdateUserInfoReqDto payload, CancellationToken cancellationToken = default!);
    
    Task<UserInfoDetailResBaseDto> GetUserInfoByIdAsync(string id, CancellationToken cancellationToken = default!);
    
    Task<bool> DeleteUserInfoAsync(DeleteUserInfoReqDto payload, CancellationToken cancellationToken = default!);
    
    Task<bool> CreateFromAuthRegisterAsync(AuthRegisterRequestDto payload, CancellationToken cancellationToken = default!);
    
}