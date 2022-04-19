using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class OrderDetailsDto : IDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int OrderStatus { get; set; }
        public decimal PayBack { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<OrderItemDetail> OrderItems { get; set; }
    }
}
