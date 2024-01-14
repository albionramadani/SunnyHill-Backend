using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Address { get; set; } = default!;
        public Gender Gender { get; set; } = default!;
        public DateTime BirthDay { get; set; } = default!;

        public List<UserProducts> FavoriteProducts { get; set; } = new List<UserProducts>();

    }
}
