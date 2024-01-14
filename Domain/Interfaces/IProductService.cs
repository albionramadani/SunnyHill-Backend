using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductService
    {
        public Task<List<ProductModel>> GetAllAsync(bool asWishList, CancellationToken token);
        public Task<ProductModel> AddOrUpdateAsync(ProductModel model,CancellationToken token);

        public Task AddProductToWishList(Guid productId, CancellationToken token);
        public Task<ProductModel> GetById(Guid id, CancellationToken token);
        public Task DeleteAsync(Guid id,CancellationToken token);

    }
}
