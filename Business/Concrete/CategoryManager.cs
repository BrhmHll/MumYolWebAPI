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
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;
        IProductDal _productDal;
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
            category.Id = 0;
            _categoryDal.Add(category);
            return new SuccessResult("Kategori Eklendi");
        }

        [ValidationAspect(typeof(CategoryValidator))]
        [SecuredOperation("personnel,admin")]
        [CacheRemoveAspect("ICategoryService.Get")]
        public IResult Update(Category category)
        {
            _categoryDal.Update(category);
            return new SuccessResult("Kategori Guncellendi");
        }

        [CacheAspect]
        [SecuredOperation("user,personnel,admin")]
        public IDataResult<List<Category>> GetAll()
        {
            return new SuccessDataResult<List<Category>>(_categoryDal.GetAll());
        }

        [CacheAspect]
        [SecuredOperation("user,personnel,admin")]
        public IDataResult<List<Category>> GetAllByIsWholesale(int topCategoryId)
        {
           return new SuccessDataResult<List<Category>>(_categoryDal.GetAll(c => c.TopCategoryId == topCategoryId || c.TopCategoryId.Equals(3)));
        }

        public IDataResult<Category> GetById(int categoryId)
        {
            var cat = _categoryDal.Get(c => c.Id.Equals(categoryId));
            if (cat == null)
                return new ErrorDataResult<Category>("Kategori bulunamadi!");
            return new SuccessDataResult<Category>(cat);
        }
    }
}
