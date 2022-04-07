using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.DTOs;

namespace Business.Concrete
{
	public class AuthManager : IAuthService
    {
        private IUserService _panelUserService;
        private ITokenHelper _tokenHelper;

        public AuthManager(IUserService panelUserService, ITokenHelper tokenHelper)
        {
            _panelUserService = panelUserService;
            _tokenHelper = tokenHelper;
        }

        [ValidationAspect(typeof(UserForRegisterDtoValidator))]
        public IDataResult<User> Register(UserForRegisterDto panelUserForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var panelUser = new User
            {
                Email = panelUserForRegisterDto.Email,
                FirstName = panelUserForRegisterDto.FirstName,
                LastName = panelUserForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneNumber = panelUserForRegisterDto.PhoneNumber,
                Address = panelUserForRegisterDto.Address,
                Status = true
            };
            _panelUserService.Add(panelUser);
            return new SuccessDataResult<User>(panelUser, Messages.UserRegistered);
        }
        [ValidationAspect(typeof(UserForLoginDtoValidator))]
        public IDataResult<User> Login(UserForLoginDto panelUserForLoginDto)
        {
            var userToCheck = _panelUserService.GetByPhoneNumber(panelUserForLoginDto.PhoneNumber);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(panelUserForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck, Messages.SuccessfulLogin);
        }

        public IResult UserExists(string phoneNumber)
        {
            if (_panelUserService.GetByPhoneNumber(phoneNumber) != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User panelUser)
        {
            var claims = _panelUserService.GetClaims(panelUser);
            var accessToken = _tokenHelper.CreateToken(panelUser, claims);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }
    }
}
