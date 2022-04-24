using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface ISettingService
    {
        bool Add(Setting setting);
        bool Modify(Setting setting);
        List<Setting> GetAll();
        string Get(string key);
        bool Update(Setting setting);
    }
}
