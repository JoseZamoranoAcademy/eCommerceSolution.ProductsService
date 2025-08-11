using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace BusinessLogicLayer.ServiceContracts
{
    public interface IProductService
    {
        /// <summary>
        /// Retrieves the list of products from the products repository
        /// </summary>
        /// <returns> Returns list of ProductResponse objects </returns>
        Task<List<ProductResponse>> GetProducts();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionExpression"></param>
        /// <returns>Returns matching products</returns>
        Task<List<ProductResponse?>> GetProductNamesByCondition(string SearchString);


        Task<List<ProductResponse?>> GetProductCategpriesByCondition(string SearchString);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionExpression"></param>
        /// <returns></returns>
        Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression);
        
        /// <summary>
        /// Adds (insert products)
        /// </summary>
        /// <param name="productAddRequest"></param>
        /// <returns></returns>
        Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest);
        Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest);
        
        /// <summary>
        /// Deletes an existing product based on their ID
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        Task<bool> DeleteProduct(Guid productID);
    }
}
