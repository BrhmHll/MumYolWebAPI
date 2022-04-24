using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheRemoveAspect : MethodInterception
    {
        private string[] _patterns;
        private ICacheManager _cacheManager;

        public CacheRemoveAspect(string pattern)
        {
            _patterns = pattern.Split("|");
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            foreach (var pattern in _patterns)
                _cacheManager.RemoveByPattern(pattern);
        }
    }
}
