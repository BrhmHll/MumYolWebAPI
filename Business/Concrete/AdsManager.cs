using Business.Abstract;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Helper;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AdsManager : IAdsService
    {
        IAdsDal _adsDal;

        public AdsManager(IAdsDal adsDal)
        {
            _adsDal = adsDal;
        }

        [CacheRemoveAspect("IAdsService.Get")]
        public IResult Add(IFormFile image)
        {
            var imageResult = FileHelper.Upload(image);
            if (imageResult == null) return imageResult;
            var ads = new Ads()
            {
                ImagePath = imageResult.Message,
                CreatedDate = DateTime.Now,
            };
            _adsDal.Add(ads);
            return new SuccessResult("Reklam başarıyla yüklendi");
        }

        [CacheAspect]
        public IDataResult<List<Ads>> GetAll()
        {
            var ads = _adsDal.GetAll(); 
            return new SuccessDataResult<List<Ads>>(ads);
        }

        [CacheRemoveAspect("IAdsService.Get")]
        public IResult Remove(int id)
        {
            var ads = _adsDal.Get(a => a.Id == id);
            if(ads==null) return new ErrorResult("Reklam bulunamadı!");
            _adsDal.Delete(ads);
            return new SuccessResult("Başarıyla silindi");
        }
    }
}
