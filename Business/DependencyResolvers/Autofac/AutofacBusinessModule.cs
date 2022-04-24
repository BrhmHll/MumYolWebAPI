using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
	public class AutofacBusinessModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			// Bu projeye ozel resolverlar burada olusturulur
			builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();
			builder.RegisterType<AuthManager>().As<IAuthService>().SingleInstance();
			builder.RegisterType<UserManager>().As<IUserService>().SingleInstance();
			builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
			builder.RegisterType<BasketManager>().As<IBasketService>().SingleInstance();
			builder.RegisterType<BalanceHistoryManager>().As<IBalanceHistoryService>().SingleInstance();
			builder.RegisterType<OrderManager>().As<IOrderService>().SingleInstance();
			builder.RegisterType<ProductImageManager>().As<IProductImageService>().SingleInstance();
			builder.RegisterType<SettingManager>().As<ISettingService>().SingleInstance();




			builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();
			builder.RegisterType<EfBasketItemDal>().As<IBasketItemDal>().SingleInstance();
			builder.RegisterType<EfCategoryDal>().As<ICategoryDal>().SingleInstance();
			builder.RegisterType<EfOrderDal>().As<IOrderDal>().SingleInstance();
			builder.RegisterType<EfOrderItemDal>().As<IOrderItemDal>().SingleInstance();
			builder.RegisterType<EfProductImageDal>().As<IProductImageDal>().SingleInstance();
			builder.RegisterType<EfUserDal>().As<IUserDal>().SingleInstance();
			builder.RegisterType<EfBalanceHistoryDal>().As<IBalanceHistoryDal>().SingleInstance();
			builder.RegisterType<EfSettingDal>().As<ISettingDal>().SingleInstance();



			//IVehicleService _vehicleService;
			//IServiceProcessService _serviceProcessService;

			builder.RegisterType<JwtHelper>().As<ITokenHelper>().SingleInstance();

			// For our interceptors to work
			var assembly = System.Reflection.Assembly.GetExecutingAssembly();
			builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
				.EnableInterfaceInterceptors(new ProxyGenerationOptions()
			{
				Selector = new AspectInterceptorSelector()
			}).SingleInstance();


		}
	}
}
