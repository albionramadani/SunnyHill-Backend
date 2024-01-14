using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Namotion.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configuration
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
           builder.HasKey(x => x.Id);
           builder.Property(x => x.Name).HasMaxLength(Lengths.MaxNameLength);
           builder.Property(x => x.Description).HasMaxLength(Lengths.MaxDescriptionLength);
        }
    }
}
