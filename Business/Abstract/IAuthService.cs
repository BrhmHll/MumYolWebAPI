using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.DTOs;

namespace Business.Abstract
{
	public interface IAuthService
    {
        IResult RegisterPhone(string phone);
        IResult CheckPhone(string phoneNumber, string verificationCode);
        IDataResult<User> Register(UserForRegisterDto panelUserForRegisterDto);
        IDataResult<User> Login(UserForLoginDto panelUserForLoginDto);
        IResult UserExists(string phoneNumber);
        IDataResult<AccessToken> CreateAccessToken(User panelUser);
        IDataResult<User> GetLastRegisteredUser();
    }
}
