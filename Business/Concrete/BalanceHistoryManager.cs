﻿using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
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
        public IDataResult<List<BalanceHistory>> GetAllByUserId(int userId)
        {
            return new SuccessDataResult<List<BalanceHistory>>(_balanceHistoryDal.GetAll(b => b.UserId.Equals(userId)));
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
