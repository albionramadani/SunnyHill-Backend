using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public static class ProductMappers
    {
        public static ProductModel MapEnitityToModel(this ProductEntity entity ,Guid? userId)
        {
            var model = new ProductModel();
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.Description = entity.Description;
            model.Price = entity.Price;
            model.IsFavorite = entity.UserProducts.Where(x=> x.UserId == userId).Any();

            return model;

        }

        public static ProductEntity MapModelToEntity(this ProductEntity entity, ProductModel model)
        {
            
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Price = model.Price;

            return entity;

        }

    }
}
