using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class ProductDetailDto : Product
    {
        public string CategoryName { get; set; }
        public List<string> ImagePaths { get; set; }
    }
}
