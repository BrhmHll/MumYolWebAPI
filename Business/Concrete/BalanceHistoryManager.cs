using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BalanceHistoryManager : IBalanceHistoryService
    {
        IBalanceHistoryDal _balanceHistoryDal;
        IUserService _userService;

        public BalanceHistoryManager(IBalanceHistoryDal balanceHistoryDal, IUserService userService)
        {
            _balanceHistoryDal = balanceHistoryDal;
            _userService = userService;
        }

        [SecuredOperation("personnel,admin")]
        [CacheRemoveAspect("IBalanceHistoryService.Get")]
        public IResult Add(BalanceHistory balanceHistory)
        {
            balanceHistory.Id = 0;
            _balanceHistoryDal.Add(balanceHistory);
            return new SuccessResult("Kategori Eklendi");
        }

        [SecuredOperation("personnel,admin")]
        [CacheRemoveAspect("IBalanceHistoryService.Get")]
        public IResult Update(BalanceHistory balanceHistory)
        {
            _balanceHistoryDal.Update(balanceHistory);
            return new SuccessResult("Kategori Guncellendi");
        }

        [CacheAspect]
        [SecuredOperation("user,personnel,admin")]
        public IDataResult<List<BalanceHistory>> GetAll()
        {
            return new SuccessDataResult<List<BalanceHistory>>(_balanceHistoryDal.GetAll(b => b.UserId.Equals(_userService.GetUser().Id)));
        }

        [CacheAspect]
        [SecuredOperation("user,personnel,admin")]
        public IDataResult<BalanceHistory> GetById(int balanceHistoryId)
        {
            var cat = _balanceHistoryDal.Get(c => c.Id.Equals(balanceHistoryId));
            if (cat == null)
                return new ErrorDataResult<BalanceHistory>("balance id bulunamadi!");
            return new SuccessDataResult<BalanceHistory>(cat);
        }
    }
}
