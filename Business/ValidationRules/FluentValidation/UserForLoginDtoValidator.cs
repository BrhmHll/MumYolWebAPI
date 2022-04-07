using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
	public class UserForLoginDtoValidator : AbstractValidator<UserForLoginDto>
	{
		public UserForLoginDtoValidator()
		{
			RuleFor(p => p.PhoneNumber).NotEmpty();
			RuleFor(p => p.PhoneNumber).Length(10);
			RuleFor(p => p.Password).NotEmpty();
			RuleFor(p => p.Password).MinimumLength(6);


		}
	}
}
