using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer.RepositoryContracts
{
    /// <summary>
    /// Represents a repository for managing products table
    /// </summary>
    public interface IProductsRepository
    {
        /// <summary>
        /// Retrieves all products asynchronously 
        /// </summary>
        /// <returns> returns all products from the table</returns>
        Task<IEnumerable<Product>> GetProducts();

        /// <summary>
        /// Retrieve all
        /// </summary>
        /// <param name="conditionExpression"></param>
        /// <returns></returns>
        Task<IEnumerable<Product?>> GetProductNamesByCondition(string SearchString);


        Task<IEnumerable<Product?>> GetProductCategoriesByCondition(string SearchString);


        /// <summary>
        /// Retrieves a signle product by a condition specified asyncrhonosly
        /// </summary>
        /// <param name="conditionExpression"></param>
        /// <returns></returns>
        Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression);
    
        Task<Product?> AddProduct(Product product);

        Task<Product?> UpdateProduct(Product product);

        Task<bool> DeleteProduct(Guid productID);


    }
}
