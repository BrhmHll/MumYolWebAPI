using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
	public class Product : IEntity
	{
		public int Id { get; set; }
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public string Brand { get; set; }
		public string Description { get; set; }
		public string Unit { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal WholesalePrice { get; set; }
        public decimal RetailPrice { get; set; }
		public int MinQuantityForWholesale { get; set; }
        public int PayBackRate { get; set; }
        public int StockAmount { get; set; }
		public bool IsActive { get; set; }
	}
}
