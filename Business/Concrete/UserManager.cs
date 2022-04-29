using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.IoC;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class UserManager : IUserService
    {
        IUserDal _panelUserDal;
        IHttpContextAccessor _contextAccessor;
        IBalanceHistoryService _balanceHistoryService;

        public UserManager(IUserDal panelUserDal, IBalanceHistoryService balanceHistoryService)
        {
            _panelUserDal = panelUserDal;
            _contextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            _balanceHistoryService = balanceHistoryService;
        }

        public List<OperationClaim> GetClaims(User panelUser)
        {
            return _panelUserDal.GetClaims(panelUser);
        }

        public IResult Add(User panelUser)
        {
            if (_panelUserDal.Get(u => u.PhoneNumber == panelUser.PhoneNumber) != null)
                return new ErrorResult("Bu telefon numarası zaten kayıtlı!");
            _panelUserDal.Add(panelUser);
            _panelUserDal.AddUserRole(panelUser.Id);
            return new SuccessResult("Kullanici eklendi");
        }

        public IDataResult<User> GetByMail(string email)
        {
            var user = _panelUserDal.Get(u => (u.Email == email) && u.Status);
            if (user == null)
                return new ErrorDataResult<User>("Bu maile ait kulanici bulunamadi!");
            return new SuccessDataResult<User>(user);

        }

        public IDataResult<User> GetByPhoneNumber(string phoneNumber)
        {
            var user = _panelUserDal.Get(u => u.PhoneNumber == phoneNumber && u.Status);
            if (user == null)
                return new ErrorDataResult<User>("Bu telefona ait kulanici bulunamadi!");
            return new SuccessDataResult<User>(user);
        }

        //[SecuredOperation("personnel,admin")]
        //[ValidationAspect(typeof(ProductValidator))]
        //[CacheRemoveAspect("IProductService.Get")]
        //[TransactionScopeAspect]
        public User GetUser()
        {
            var userId = Convert.ToInt32(_contextAccessor.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            return _panelUserDal.Get(u => u.Id == userId);
        }

        public User GetAdmin()
        {
            return _panelUserDal.GetAdmin();
        }

        public IDataResult<User> GetUserById(int userId)
        {
            var user = _panelUserDal.Get(u => u.Id == userId);
            if (user == null)
                return new ErrorDataResult<User>("Bu maile ait kulanici bulunamadi!");
            return new SuccessDataResult<User>(user);
        }


        [SecuredOperation("personnel,admin")]
        public IResult Update(User user)
        {
            var userr = _panelUserDal.Get(u => u.Id.Equals(user.Id));
            if (userr == null)
                return new ErrorResult("Kullanıcı Bulunamadı!");
            _panelUserDal.Update(user);
            return new SuccessResult("Kullanici guncellendi");

        }

        public IDataResult<UserProfileDto> GetUserProfile()
        {
            var user = GetUser();
            if (user == null) return new ErrorDataResult<UserProfileDto>("Kullanici bulunamadi!");
            var userProfile = new UserProfileDto()
            {
                Address = user.Address,
                Balance = user.Balance,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
            };
            return new SuccessDataResult<UserProfileDto>(userProfile);
        }

        public IDataResult<UserProfileDto> UpdateUserProfile(UserForUpdateDto user)
        {
            var userOriginal = GetUser();
            if (userOriginal == null) return new ErrorDataResult<UserProfileDto>("Kullanici bulunamadi!");
            userOriginal.Address = user.Address;
            userOriginal.FirstName = user.FirstName;
            userOriginal.LastName = user.LastName;
            userOriginal.Email = user.Email;
            _panelUserDal.Update(userOriginal);
            return GetUserProfile();
        }

        public IDataResult<List<UserProfileDto>> GetAllUserProfile()
        {
            var users = _panelUserDal.GetAll();
            var usersProfiles = new List<UserProfileDto>();
            foreach (var user in users)
            {
                usersProfiles.Add(new UserProfileDto()
                {
                    Address=user.Address,
                    Balance=user.Balance,
                    Email=user.Email,
                    FirstName=user.FirstName,
                    LastName=user.LastName,
                    PhoneNumber=user.PhoneNumber,
                    Status = user.Status,
                    Id = user.Id,
                });
            }
            return new SuccessDataResult<List<UserProfileDto>>(usersProfiles);
        }

        public IResult UpdateUserStatus(UserStatusUpdateDto userStatusUpdateDto)
        {
            var user = _panelUserDal.Get(u => u.Id.Equals(userStatusUpdateDto.UserId));
            if(user  == null)
                return new ErrorResult("Kullanıcı bulunamadı!");
            user.Status = userStatusUpdateDto.Status;
            _panelUserDal.Update(user);
            return new SuccessResult("Kullanıcı durumu " + (user.Status ? "Aktif" : "Pasif") + " olarak ayarlandı.");
        }

        [SecuredOperation("admin,personnel")]
        public IResult PaybackPayment(PaybackPaymentDto paybackPaymentDto)
        {
            var user = _panelUserDal.Get(u => u.Id.Equals(paybackPaymentDto.UserId));
            if (user == null)
                return new ErrorResult("Kullanıcı bulunamadı!");
            var balanceHistory = new BalanceHistory()
            {
                Money = 0 - paybackPaymentDto.PaybackAmount,
                BalanceAfter = user.Balance - paybackPaymentDto.PaybackAmount,
                Date = DateTime.Now,
                UserId = paybackPaymentDto.UserId,
            };
            if (balanceHistory.BalanceAfter < 0)
            {
                return new ErrorResult("Hesap bakiyesinden fazla ödeme yapamazsınız!\nBakiye: " + user.Balance.ToString());
            }

            _balanceHistoryService.Add(balanceHistory);

            user.Balance = balanceHistory.BalanceAfter;
            _panelUserDal.Update(user);
            return new SuccessResult("Ödeme gerçekleşti.\n" + user.FirstName + " " + user.LastName + " Tutar: " + paybackPaymentDto.PaybackAmount.ToString());
        }

        [SecuredOperation("admin,personnel,user")]
        public IDataResult<List<BalanceHistory>> GetAllBalanceHistories()
        {
            var histories = _balanceHistoryService.GetAllByUserId(GetUser().Id);
            return new SuccessDataResult<List<BalanceHistory>>(histories.Data);
        }
    }
}
