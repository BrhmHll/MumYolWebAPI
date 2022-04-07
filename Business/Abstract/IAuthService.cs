using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.DTOs;

namespace Business.Abstract
{
	public interface IAuthService
    {
        IDataResult<User> Register(UserForRegisterDto panelUserForRegisterDto, string password);
        IDataResult<User> Login(UserForLoginDto panelUserForLoginDto);
        IResult UserExists(string phoneNumber);
        IDataResult<AccessToken> CreateAccessToken(User panelUser);
    }
}
