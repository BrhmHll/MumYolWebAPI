using Core.Entities.Concrete;
using Core.Utilities.IoC;
using Entities;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
	public class MumYolContext : DbContext
	{
		protected readonly IConfiguration Configuration;

		public MumYolContext()
		{
			Configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			//optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MumYol;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
		}

		public DbSet<OperationClaim> OperationClaims { get; set; }
		public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
		public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
		public DbSet<BasketItem> BasketItems { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Setting> Settings { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<BalanceHistory> BalanceHistories { get; set; }
		public DbSet<Ads> Ads { get; set; }




	}
}
