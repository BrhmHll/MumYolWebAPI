using Core.Entities;

namespace Entities.DTOs
{
	public class UserProfileDto : UserForUpdateDto
    {
        public decimal Balance { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }

    }
}
