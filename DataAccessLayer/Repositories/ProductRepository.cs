using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Product?> AddProduct(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return product;
        }

        public async Task<bool> DeleteProduct(Guid productID)
        {
            Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(temp =>temp.ProductID == productID);

            if (existingProduct is null)
            {
                return false;
            }
            _dbContext.Products.Remove(existingProduct);
            int affectedRowsCount = await _dbContext.SaveChangesAsync();

            return (affectedRowsCount > 0);
        }


        public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
        {
            return await _dbContext.Products.FirstOrDefaultAsync<Product>(conditionExpression);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product?>> GetProductNamesByCondition(string SearchString)
        {
            var searchValue = SearchString.ToLower().Trim();
            var selectedProducts = _dbContext.Products.Where(a => a.ProductName.Trim().ToLower().Contains(searchValue));
            var selectedProductsList = selectedProducts.Count() > 0 ? selectedProducts.ToList() : new List<Product>();
            return selectedProductsList;   
        }

        public async Task<IEnumerable<Product?>> GetProductCategoriesByCondition(string SearchString)
        {
            var selectedProducts = _dbContext.Products.Where(a => a.Category.Trim().ToLower().Contains(SearchString.Trim().ToLower())).ToList();

            return selectedProducts;
        }

        public async Task<Product?> UpdateProduct(Product product)
        {
            Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductID == product.ProductID);
        
            if (existingProduct is null)
            {  
                return null; 
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.UnitPrice = product.UnitPrice;
            existingProduct.QuantityInStock = product.QuantityInStock;
            existingProduct.Category = product.Category;

            await _dbContext.SaveChangesAsync();

            return existingProduct;        
        }
    }
}
