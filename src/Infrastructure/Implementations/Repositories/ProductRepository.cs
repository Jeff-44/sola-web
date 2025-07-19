using ApplicationCore.Interfaces.IRepository;
using ApplicationCore.Models;
using Infrastructure.Data;

namespace Infrastructure.Implementations.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(SolaContext context) : base(context)
        {
        }
    }
}
