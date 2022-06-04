using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
	public class UserForRegisterDtoValidator : AbstractValidator<UserForRegisterDto>
	{
		public UserForRegisterDtoValidator()
		{
			RuleFor(p => p.PhoneNumber).NotEmpty();
			RuleFor(p => p.PhoneNumber).Length(10);
			RuleFor(p => p.Password).NotEmpty();
			RuleFor(p => p.Password).MinimumLength(6);
			RuleFor(p => p.FirstName).NotEmpty();
			RuleFor(p => p.FirstName).MinimumLength(2);
			RuleFor(p => p.LastName).NotEmpty();
			RuleFor(p => p.LastName).MinimumLength(2);
			//RuleFor(p => p.Address).NotEmpty();
			//RuleFor(p => p.Address).MinimumLength(10);
			//RuleFor(p => p.Email).Must(MailIsValid);
		}

		public bool MailIsValid(string emailaddress)
		{
			try
			{
				MailAddress m = new MailAddress(emailaddress);

				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}
	}
}
