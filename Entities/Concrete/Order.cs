using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
	public class Order : IEntity
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public DateTime CreatedDate { get; set; }
		public int OrderStatus { get; set; }
        public decimal PayBack { get; set; }
        public string Address { get; set; }

	}
}
