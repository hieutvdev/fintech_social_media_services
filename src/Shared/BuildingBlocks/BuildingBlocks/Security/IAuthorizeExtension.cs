using ShredKernel.BaseClasses;

namespace BuildingBlocks.Security;

public interface IAuthorizeExtension
{
      UserLoginResponseBase GetUserFromClaimToken();
      bool ValidateToken(string token);
      UserLoginResponseBase DecodeToken(string token);
}