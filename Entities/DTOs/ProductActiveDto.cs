using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
	public class ProductActiveDto : IDto
    {
        public int ProductId { get; set; }
        public bool IsActive { get; set; }
    }
}
