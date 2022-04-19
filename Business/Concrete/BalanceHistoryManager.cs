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
        public BalanceHistoryManager(IBalanceHistoryDal balanceHistoryDal)
        {
            _balanceHistoryDal = balanceHistoryDal;
        }

        [SecuredOperation("personnel,admin")]
        [CacheRemoveAspect("ICategoryService.Get")]
        public IResult Add(BalanceHistory category)
        {
            category.Id = 0;
            _balanceHistoryDal.Add(category);
            return new SuccessResult("Kategori Eklendi");
        }

        [SecuredOperation("personnel,admin")]
        [CacheRemoveAspect("ICategoryService.Get")]
        public IResult Update(BalanceHistory category)
        {
            _balanceHistoryDal.Update(category);
            return new SuccessResult("Kategori Guncellendi");
        }

        [CacheAspect]
        [SecuredOperation("user,personnel,admin")]
        public IDataResult<List<BalanceHistory>> GetAll()
        {
            return new SuccessDataResult<List<BalanceHistory>>(_balanceHistoryDal.GetAll());
        }

        [CacheAspect]
        [SecuredOperation("user,personnel,admin")]
        public IDataResult<BalanceHistory> GetById(int balanceHistoryId)
        {
            var cat = _balanceHistoryDal.Get(c => c.Id.Equals(balanceHistoryId));
            if (cat == null)
                return new ErrorDataResult<BalanceHistory>("Kategori bulunamadi!");
            return new SuccessDataResult<BalanceHistory>(cat);
        }
    }
}
