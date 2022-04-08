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
	public interface IBasketService
    {
        IResult Add(BasketDto basketDto);
        IResult Update(BasketItem basketItem);
        IResult Delete(int basketItemId);
        IResult DeleteAll();
        IDataResult<List<BasketItem>> GetAll();
        IDataResult<List<BasketDetailsDto>> GetAllDetails();
    }
}
