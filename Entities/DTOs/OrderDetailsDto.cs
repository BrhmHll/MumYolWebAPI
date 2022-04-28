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
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public int OrderStatus { get; set; }
        public decimal PayBack { get; set; }
        public decimal Cost { get; set; }
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<OrderItemDetail> OrderItems { get; set; }
    }
}
