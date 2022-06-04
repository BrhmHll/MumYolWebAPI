using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
	public class EfUserDal : EfEntityRepositoryBase<User, MumYolContext>, IUserDal
	{
		public List<OperationClaim> GetClaims(User user)
		{
			using (var context = new MumYolContext())
			{
				var result = from operationClaim in context.OperationClaims
							 join userOperationClaim in context.UserOperationClaims
								 on operationClaim.Id equals userOperationClaim.OperationClaimId
							 where userOperationClaim.UserId == user.Id
							 select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
				return result.ToList();

			}
		}

		public void AddUserRole(int userId)
        {
			using (var context = new MumYolContext())
			{
				var addedEntity = context.Entry(new UserOperationClaim() { 
					OperationClaimId = 3,
					UserId = userId
				});
				addedEntity.State = EntityState.Added;
				context.SaveChanges();
			}
		}

		public User GetAdmin()
		{
			using (var context = new MumYolContext())
			{
				var user = from UserOperationClaim in context.UserOperationClaims
						   join User in context.Users
							on UserOperationClaim.UserId equals User.Id
						   where UserOperationClaim.OperationClaimId == 1
						   select new User
                           {
							   Id = User.Id,
							   //Address = User.Address,
							   CreatedDate = User.CreatedDate,
							   //Email = User.Email,
							   FirstName = User.FirstName,
							   LastName = User.LastName,
							   PhoneNumber = User.PhoneNumber,
							   PhoneNumberVerificated = User.PhoneNumberVerificated,
							   Status = User.Status,
							   VerificationCode = User.VerificationCode,
                           };
				return user.FirstOrDefault();
			}
		}

		public string GetSetting(string key)
        {
			using (var context = new MumYolContext())
			{
				var value = from s in context.Settings
						   where s.Key.Equals(key)
						   select s.Value;
						   
				return value.FirstOrDefault();
			}
		}
	}
}
