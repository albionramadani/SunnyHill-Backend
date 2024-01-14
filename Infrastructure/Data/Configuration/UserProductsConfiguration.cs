using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configuration
{
    public class UserProductsConfiguration : IEntityTypeConfiguration<UserProducts>
    {
        public void Configure(EntityTypeBuilder<UserProducts> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.ProductEntity)
                .WithMany(x => x.UserProducts)
                .HasForeignKey(x => x.ProductId);

            builder.HasOne(x => x.User)
              .WithMany(x => x.FavoriteProducts)
              .HasForeignKey(x => x.UserId);
        }
    }
}
