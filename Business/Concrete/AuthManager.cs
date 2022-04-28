using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Integrations;
using Core.Utilities.IoC;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Concrete
{
	public class AuthManager : IAuthService
    {
        private IUserService _panelUserService;
        private ITokenHelper _tokenHelper;
        private ICacheManager _cacheManager;


        public AuthManager(IUserService panelUserService, ITokenHelper tokenHelper)
        {
            _panelUserService = panelUserService;
            _tokenHelper = tokenHelper;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        [ValidationAspect(typeof(UserForRegisterDtoValidator))]
        public IDataResult<User> Register(UserForRegisterDto panelUserForRegisterDto)
        {
            //For test its closed
            //var key = string.Format(Messages.VerificationCodeWithPhoneKey, panelUserForRegisterDto.PhoneNumber);
            //var verificationCode = _cacheManager.Get(key).ToString();
            //if (verificationCode == null)
            //    return new ErrorDataResult<User>("Doğrulama kodu bulunamadı veya zaman aşımına uğradı! Lütfen tekrar onay kodu alınız.");
            //if (panelUserForRegisterDto.VerificationCode != verificationCode)
            //    return new ErrorDataResult<User>("Hatalı doğrulama kodu! Lütfen tekrar deneyiniz.");
            //var res = CheckPhone(panelUserForRegisterDto.PhoneNumber, panelUserForRegisterDto.VerificationCode);
            //if (!res.Success)
            //    return new ErrorDataResult<User>(res.Message);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(panelUserForRegisterDto.Password, out passwordHash, out passwordSalt);
            var panelUser = new User
            {
                Email = panelUserForRegisterDto.Email,
                FirstName = panelUserForRegisterDto.FirstName,
                LastName = panelUserForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneNumber = panelUserForRegisterDto.PhoneNumber,
                Address = panelUserForRegisterDto.Address,
                Status = true,
                CreatedDate = System.DateTime.Now,
                PhoneNumberVerificated = false,
                VerificationCode = panelUserForRegisterDto.VerificationCode,
            };
            var res = _panelUserService.Add(panelUser);
            if(!res.Success)
                return new ErrorDataResult<User>(res.Message);
            _cacheManager.Add("last_user", panelUser, 60);
            
            return new SuccessDataResult<User>(panelUser, Messages.UserRegistered);
        }
        [ValidationAspect(typeof(UserForLoginDtoValidator))]
        public IDataResult<User> Login(UserForLoginDto panelUserForLoginDto)
        {
            var userToCheck = _panelUserService.GetByPhoneNumber(panelUserForLoginDto.PhoneNumber);
            if (!userToCheck.Success)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(panelUserForLoginDto.Password, userToCheck.Data.PasswordHash, userToCheck.Data.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck.Data, Messages.SuccessfulLogin);
        }

        public IResult UserExists(string phoneNumber)
        {
            if (_panelUserService.GetByPhoneNumber(phoneNumber).Success)
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

        public IResult RegisterPhone(string phoneNumber)
        {
            if (phoneNumber.Length != 10) return new ErrorResult("Telefon uzunluğu 10 haneli olmalıdır!");

            var code = SmsIntegration.GenerateCode();
            var msg = string.Format(Messages.VerificationMessage, code);
            //SmsIntegration.SendSms(phoneNumber, msg);
            var key = string.Format(Messages.VerificationCodeWithPhoneKey, phoneNumber);
            _cacheManager.Add(key, code, 10);
            return new SuccessResult("Doğrulama kodu " + code + " gönderildi: " + phoneNumber);
        }

        public IResult CheckPhone(string phoneNumber, string verificationCode)
        {
            if(phoneNumber.Length != 10) return new ErrorResult("Telefon uzunluğu 10 haneli olmalıdır!");
            if(verificationCode.Length != 6) return new ErrorResult("Onay kodu uzunluğu 6 haneli olmalıdır!");

            var key = string.Format(Messages.VerificationCodeWithPhoneKey, phoneNumber);
            var cachedVerificationCode = _cacheManager.Get(key);
            if (cachedVerificationCode == null)
                return new ErrorResult("Doğrulama kodu bulunamadı veya zaman aşımına uğradı! Lütfen tekrar onay kodu alınız.");
            if (cachedVerificationCode.ToString() != verificationCode)
                return new ErrorResult("Hatalı doğrulama kodu! Lütfen tekrar deneyiniz.");
            return new SuccessResult("Telefon doğrulama başarılı!");
        }

        public IDataResult<User> GetLastRegisteredUser()
        {
            var user = _cacheManager.Get<User>("last_user");
            return new SuccessDataResult<User>(user);
        }

        [SecuredOperation("admin,personnel")]
        public IResult ResetPasswordByAdmin(int userId)
        {
            var user = _panelUserService.GetUserById(userId);
            if (!user.Success) return new ErrorResult(user.Message);


            var psw = SmsIntegration.CreateRandomPassword();
            var msg = string.Format(Messages.NewPasswordMessage, psw);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(psw, out passwordHash, out passwordSalt);
            user.Data.PasswordSalt = passwordSalt;
            user.Data.PasswordHash = passwordHash;
            _panelUserService.Update(user.Data);
            SmsIntegration.SendSms(user.Data.PhoneNumber, msg);

            return new SuccessResult("Şifre sıfırlandı!");
        }
    }
}
