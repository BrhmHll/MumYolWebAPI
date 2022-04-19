using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Core.Aspects.Autofac.Caching;
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
	public class BasketManager : IBasketService
    {
        IBasketItemDal _basketItemDal;
        IUserService _userService;
        IProductService _productService;

        public BasketManager(IBasketItemDal basketItemDal, IUserService userService, IProductService productService)
        {
            _basketItemDal = basketItemDal;
            _userService = userService;
            _productService = productService;
        }

        [SecuredOperation("user,personnel,admin")]
        public IResult Add(BasketDto basketDto)
        {
            var user = _userService.GetUser();
            var productExists = _productService.GetById(basketDto.ProductId);
            if (!productExists.Success)
                return new ErrorResult("Urun bulunamadi!");

            var item =  _basketItemDal.Get(b => b.UserId.Equals(user.Id) && b.ProductId.Equals(basketDto.ProductId));
            if (item == null)
            {
                _basketItemDal.Add(new BasketItem()
                {
                    ProductId = basketDto.ProductId,
                    Quantity = basketDto.Quantity,
                    UserId = user.Id,
                });
            }
            else
            {
                item.Quantity += basketDto.Quantity;
                _basketItemDal.Update(item);
            }

            return new SuccessResult("Sepete Eklendi!");
        }

        [SecuredOperation("user,personnel,admin")]
        public IResult Delete(int basketItemId)
        {
            var item = _basketItemDal.Get(b => b.Id.Equals(basketItemId));
            if (item == null)
                return new ErrorResult("Sepetinizdeki urun bulunamadi!");
            _basketItemDal.Delete(item);
            return new SuccessResult("Urun Silindi!");
        }

        [SecuredOperation("user,personnel,admin")]
        public IResult DeleteAll()
        {
            var items = GetAll().Data;
            foreach (var item in items)
                Delete(item.Id);
            return new SuccessResult("Sepet temizlendi!");
        }

        [SecuredOperation("user,personnel,admin")]
        public IDataResult<List<BasketItem>> GetAll()
        {
            return new SuccessDataResult<List<BasketItem>>(_basketItemDal.GetAll(b => b.UserId.Equals(_userService.GetUser().Id)));
        }

        [SecuredOperation("user,personnel,admin")]
        public IDataResult<List<BasketDetailsDto>> GetAllDetails()
        {
            return new SuccessDataResult<List<BasketDetailsDto>>(_basketItemDal.GetBasketDetails(_userService.GetUser().Id));
        }

        [SecuredOperation("user,personnel,admin")]
        public IResult Update(BasketItem basketItem)
        {
            var item = _basketItemDal.Get(b => b.Id.Equals(basketItem.Id));
            if (item == null)
                return new ErrorResult("Sepetinizdeki urun bulunamadi!");
            if(basketItem.Quantity == 0)
                _basketItemDal.Delete(item);
            else
                item.Quantity = basketItem.Quantity;
                _basketItemDal.Update(item);
            return new SuccessResult("Urun Guncellendi!");
        }
    }
}
