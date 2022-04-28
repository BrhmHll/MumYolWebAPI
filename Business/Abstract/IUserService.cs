using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.DTOs;
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
        IResult Add(User panelUser);
        IResult Update(User user);
        IDataResult<User> GetByMail(string email);
        IDataResult<User> GetByPhoneNumber(string phoneNumber);
        User GetUser();
        IDataResult<User> GetUserById(int userId);
        IDataResult<UserProfileDto> GetUserProfile();
        IDataResult<List<UserProfileDto>> GetAllUserProfile();
        IDataResult<UserProfileDto> UpdateUserProfile(UserForUpdateDto user);
        User GetAdmin();
        IResult UpdateUserStatus(UserStatusUpdateDto userStatusUpdateDto);
    }
}
