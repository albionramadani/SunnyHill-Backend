using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserProducts
    {
        public Guid Id { get; set; }
        public Guid UserId { get;set; }
        public Guid ProductId { get; set; }

        public ProductEntity ProductEntity { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
    }
}
