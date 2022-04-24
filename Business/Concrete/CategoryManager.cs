using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Helper;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;
        IProductDal _productDal;

        private string logoPath = "/logo.png";

        public CategoryManager(ICategoryDal categoryDal, IProductDal productDal)
        {
            _categoryDal = categoryDal;
            _productDal = productDal;
        }

        [ValidationAspect(typeof(CategoryValidator))]
        [SecuredOperation("personnel,admin")]
        [CacheRemoveAspect("ICategoryService.Get")]
        public IResult Add(Category category)
        {
            var catExists = _categoryDal.GetAll(c => c.Name.Equals(category.Name)).FirstOrDefault();
            if (catExists != null) return new ErrorResult("Bu isimde bir kategori zaten mevcut!");
            category.Id = 0;
            category.ImagePath = "";
            _categoryDal.Add(category);
            return new SuccessResult("Kategori Eklendi");
        }

        [ValidationAspect(typeof(CategoryValidator))]
        [SecuredOperation("personnel,admin")]
        [CacheRemoveAspect("ICategoryService.Get")]
        public IResult Update(Category category)
        {
            var catExists = _categoryDal.GetAll(c => c.Name.Equals(category.Name)).FirstOrDefault();
            if (catExists != null) return new ErrorResult("Bu isimde bir kategori zaten mevcut!");
            _categoryDal.Update(category);
            return new SuccessResult("Kategori Guncellendi");
        }

        [CacheAspect]
        [SecuredOperation("user,personnel,admin")]
        public IDataResult<List<Category>> GetAll()
        {
            var datas = _categoryDal.GetAll();
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].ImagePath == null || datas[i].ImagePath == "") datas[i].ImagePath = logoPath;
            }
            return new SuccessDataResult<List<Category>>(datas);
        }

        public IDataResult<Category> GetById(int categoryId)
        {
            var cat = _categoryDal.Get(c => c.Id.Equals(categoryId));
            if (cat == null)
                return new ErrorDataResult<Category>("Kategori bulunamadi!");
            if(cat.ImagePath == null)
                cat.ImagePath = logoPath;
            return new SuccessDataResult<Category>(cat);
        }

        [CacheRemoveAspect("ICategoryService.Get")]
        public IResult ModifyImage(IFormFile file, int categoryId)
        {
            var cat = _categoryDal.Get(c => c.Id.Equals(categoryId));
            if(cat == null)
                return new ErrorDataResult<Category>("Kategori bulunamadi!");

            var imageResult = FileHelper.Upload(file);
            if (!imageResult.Success)
                return new ErrorResult(imageResult.Message);

            if(cat.ImagePath != null)
                FileHelper.Delete(cat.ImagePath);

            cat.ImagePath = imageResult.Message;
            var res = Update(cat);
            if (!res.Success) return res;
            return new SuccessResult("Resim guncellendi!");
        }
    }
}
