using Castle.DynamicProxy;
using Core.Aspects.Autofac.Exception;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Interceptors
{

	public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>
                (true).ToList();
            var methodAttributes = type.GetMethod(method.Name)
                .GetCustomAttributes<MethodInterceptionBaseAttribute>(true);
            classAttributes.AddRange(methodAttributes);

            //This PerformanceAspect affects all functions 
            classAttributes.Add(new PerformanceAspect(10));
            classAttributes.Add(new LogAspect(typeof(FileLogger)));
            //classAttributes.Add(new LogAspect(typeof(DatabaseLogger)));


            //Logging all exceptions
            classAttributes.Add(new ExceptionLogAspect(typeof(DatabaseLogger)));

            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }

}
