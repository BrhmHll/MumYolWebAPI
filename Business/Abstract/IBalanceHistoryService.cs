using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IBalanceHistoryService
    {
        IResult Add(BalanceHistory balanceHistory);
        IDataResult<List<BalanceHistory>> GetAllByUserId(int userId);
        IDataResult<BalanceHistory> GetById(int balanceHistoryId);
        IResult Update(BalanceHistory balanceHistory);
    }
}
