using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IAdsService
    {
        IResult Add(IFormFile image);
        IResult Remove(int id);
        IDataResult<List<Ads>> GetAll();

    }
}
