using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

       public List<UserProducts> UserProducts { get; set; } = new List<UserProducts>();
    }
}
