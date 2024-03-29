using Core.DependencyResolver;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }


		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers();


			services.AddDependencyResolvers(new ICoreModule[] {
				new CoreModule()
			});

			//services.Configure<AppSettings>(Configuration);

			//services.AddDbContext<MumYolContext>(item => item.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidIssuer = tokenOptions.Issuer,
					ValidAudience = tokenOptions.Audience,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
				};
			});


			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "MUMYOl WEB API", Version = "v0.1", Description = "<a target=\"_blank\" href=\"http://ibrahimsakar.xyz\"><h1 class=\"url\">Created by: �brahim Halil SAKAR</h1></a>" });
				
			});
			services.AddCors();


		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
				app.UseSwagger();
			
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
			}


			app.ConfigureCustomExceptionMiddleware();

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseCors(builder => builder
					.WithOrigins(new[] { "http://localhost:4200", "https://admin.momandltd.com" })
					.AllowAnyHeader()
					.AllowAnyOrigin());

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
