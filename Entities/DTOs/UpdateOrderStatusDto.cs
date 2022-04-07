using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class UpdateOrderStatusDto : IDto
    {
        public int OrderId { get; set; }
        public int StatusId { get; set; }
    }
}
