using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IUserService
    {
        List<OperationClaim> GetClaims(User panelUser);
        void Add(User panelUser);
        User GetByMail(string email);
        User GetByPhoneNumber(string phoneNumber);
        User GetUser();
    }
}
