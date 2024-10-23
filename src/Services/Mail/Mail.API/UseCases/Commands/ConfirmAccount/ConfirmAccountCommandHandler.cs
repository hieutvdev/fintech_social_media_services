﻿using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using Mail.API.Interfaces;

namespace Mail.API.UseCases.Commands.ConfirmAccount;

public class ConfirmAccountCommandHandler(ISendMailService mailService)
: ICommandHandler<ConfirmAccountCommand>
{
    public async Task<Result> Handle(ConfirmAccountCommand request, CancellationToken cancellationToken)
    {
        await mailService.SendMailConfirmAccountAsync(request.Url, cancellationToken);
        return Result.Create(true);
    }
}