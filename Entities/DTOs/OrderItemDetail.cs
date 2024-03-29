﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
	public class OrderItemDetail : IDto
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
		public decimal PurchasePrice { get; set; }
		public int PayBackRate { get; set; }
		public string ImagePath { get; set; }
        public string ProductName { get; set; }
    }
}
