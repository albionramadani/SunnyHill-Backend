using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Domain.Entities
{

    public class ApplicationDbContext : IdentityUserContext<ApplicationUser, Guid>
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> users
        {
            get { return Set<ApplicationUser>(); }

        }

        public DbSet<ProductEntity> products
        {
            get { return Set<ProductEntity>(); }

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
