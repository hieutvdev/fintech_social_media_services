namespace User.Application.DTOs.Request.Block;

public record DeleteBlockReqDto(IEnumerable<string> BlockedIds);