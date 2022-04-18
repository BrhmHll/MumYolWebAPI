using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
	public class ProductValidator : AbstractValidator<Product>
	{
		public ProductValidator()
		{
			RuleFor(p => p.Name).NotEmpty();
			RuleFor(p => p.Name).MinimumLength(2);
			RuleFor(p => p.WholesalePrice).NotEmpty();
			RuleFor(p => p.WholesalePrice).GreaterThan(0);
			RuleFor(p => p.RetailPrice).NotEmpty();
			RuleFor(p => p.RetailPrice).GreaterThan(0);
			RuleFor(p => p.MinQuantityForWholesale).NotEmpty();
			RuleFor(p => p.MinQuantityForWholesale).GreaterThan(0);
			RuleFor(p => p.Brand).NotEmpty();
			RuleFor(p => p.Brand).MinimumLength(2);
			RuleFor(p => p.StockAmount).NotEmpty();
			RuleFor(p => p.StockAmount).GreaterThan(-1);
			RuleFor(p => p.CategoryId).GreaterThan(0);

			//RuleFor(p => p.Name).Must(StartWithA).WithMessage("Ürünler A harfi ile başlamalı (Fake)");

		}

		private bool StartWithA(string arg)
		{
			return true;
		}
	}
}
