using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.IoC;
using DataAccess.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class UserManager : IUserService
    {
        IUserDal _panelUserDal;
        IHttpContextAccessor _contextAccessor;

        public UserManager(IUserDal panelUserDal)
        {
            _panelUserDal = panelUserDal;
            _contextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        public List<OperationClaim> GetClaims(User panelUser)
        {
            return _panelUserDal.GetClaims(panelUser);
        }

        public void Add(User panelUser)
        {
            _panelUserDal.Add(panelUser);
            _panelUserDal.AddUserRole(panelUser.Id);
        }

        public User GetByMail(string email)
        {
            return _panelUserDal.Get(u => (u.Email == email) && u.Status);
        }

        public User GetByPhoneNumber(string phoneNumber)
        {
            return _panelUserDal.Get(u => u.PhoneNumber == phoneNumber && u.Status);
        }

        public User GetUser()
        {
            var userId = Convert.ToInt32(_contextAccessor.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            return _panelUserDal.Get(u => u.Id == userId);
        }

        public User GetAdmin()
        {
            return _panelUserDal.GetAdmin();
        }

        public User GetUserById(int userId)
        {
            return _panelUserDal.Get(u => u.Id == userId);
        }

        public void Update(User user)
        {
            _panelUserDal.Update(user);
        }
    }
}
