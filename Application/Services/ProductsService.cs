using Application.Mappers;
using Application.Validators;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductsService : BaseDbService, IProductService
    {
        private readonly ProductValidator validator;
        private readonly IAuthorizationManager manager;
        public ProductsService(ProductValidator validator, ApplicationDbContext context,IAuthorizationManager manager): base(context)
        {
            this.validator = validator;
            this.manager = manager;
        }

        public async Task<ProductModel> AddOrUpdateAsync(ProductModel model, CancellationToken token)
        {
           var validationResult = validator.Validate(model);
            if(validationResult.IsValid) 
            {
                ProductEntity enitity = new();
                if (model.Id.HasValue)
                {
                    enitity = await _db.products.FirstAsync(x=> x.Id == model.Id,token);
                }
                else
                {
                    _db.products.Add(enitity);
                }

                enitity.MapModelToEntity(model);

                await _db.SaveChangesAsync(token);

                return await GetById(enitity.Id,token);
            }

            throw new AppBadDataException();
        }

        public async Task AddProductToWishList(Guid productId, CancellationToken token)
        {
            var userId = manager.GetUserId();
            var user = await _db.Users.Include(x=> x.FavoriteProducts).Where(x => x.Id == userId).FirstOrDefaultAsync(token);

            if(user is null)
            {
                throw new AppBadDataException();
            }
            if(user.FavoriteProducts.Any(x=> x.ProductId == productId))
            {
                user.FavoriteProducts.Remove(user.FavoriteProducts.Where(x => x.ProductId == productId).FirstOrDefault()!);
            }
            else
            {
                user.FavoriteProducts.Add(new UserProducts()
                {
                    UserId = userId ?? new()!,
                    ProductId = productId
                }) ;
            }

            await _db.SaveChangesAsync(token);

        }

        public async Task DeleteAsync(Guid id, CancellationToken token)
        {
            var enitity = await _db.products.FirstOrDefaultAsync(x => x.Id == id);

            if(enitity is null)
            {
                throw new AppNotFoundException();
            }

            _db.products.Remove(enitity);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ProductModel>> GetAllAsync(bool asWishList,CancellationToken token)
        {
            var userId = manager.GetUserId();
            var query = _db.products.Include(x=> x.UserProducts).AsQueryable();

            if (asWishList)
            {
                query = query.Where(x => x.UserProducts.Any(x => x.UserId == userId));
            }
            var models = await query.Select(x => x.MapEnitityToModel(userId)).ToListAsync(token);

            return models;
        }

        public async Task<ProductModel> GetById(Guid id, CancellationToken token)
        {
            var userId = manager.GetUserId();
            var model = await _db.products.Include(x => x.UserProducts).Where(x=> x.Id == id).Select(x => x.MapEnitityToModel(userId)).FirstOrDefaultAsync();

            return model!;
        }
    }
}
