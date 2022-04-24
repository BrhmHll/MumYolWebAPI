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
    public class SettingManager : ISettingService
    {
        ISettingDal _settingDal;
        public SettingManager(ISettingDal settingDal)
        {
            _settingDal = settingDal;
            Add(new Setting() { Key = "logo", Value = "/logo.png" });
        }

        public bool Add(Setting setting)
        {
            var settingExists = _settingDal.Get(s => s.Key.Equals(setting.Key));
            if (settingExists != null) return false;
            _settingDal.Add(setting);
            return true;
        }

        public string Get(string key)
        {
            var value = _settingDal.Get(s => s.Key.Equals(key));
            if (value != null) return value.Value;
            return null;
        }

        public List<Setting> GetAll()
        {
            return _settingDal.GetAll();
        }

        public bool Modify(Setting setting)
        {
            var settingExists = _settingDal.Get(s => s.Key.Equals(setting.Key));
            if (settingExists != null) 
                _settingDal.Update(setting);
            _settingDal.Add(setting);
            return true;
        }

        public bool Update(Setting setting)
        {
            var settingExists = _settingDal.Get(s => s.Key.Equals(setting.Key));
            if (settingExists != null) return false;
            _settingDal.Update(setting);
            return true;
        }
    }
}
