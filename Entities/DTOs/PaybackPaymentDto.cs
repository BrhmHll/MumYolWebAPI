using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class PaybackPaymentDto : IDto
    {
        public int UserId { get; set; }
        public decimal PaybackAmount { get; set; }
    }
}
