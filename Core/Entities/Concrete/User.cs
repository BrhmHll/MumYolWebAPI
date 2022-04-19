using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
	public class User : IEntity
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public byte[] PasswordSalt { get; set; }
		public byte[] PasswordHash { get; set; }
		public bool Status { get; set; }
		public bool PhoneNumberVerificated { get; set; }
		public string VerificationCode { get; set; }
		public DateTime CreatedDate { get; set; }
        public decimal Balance { get; set; }

    }
}
