using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IValidator<ProductAddRequest> _productAddRequestValidator;
        private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
        private readonly IMapper _mapper;
        private readonly IProductsRepository _productsRepository;

        public ProductService(IValidator<ProductAddRequest> productAddRequestValidator,
            IValidator<ProductUpdateRequest> productUpdateRequestValidator,
            IMapper mapper, IProductsRepository productsRepository)
        {
            _productAddRequestValidator = productAddRequestValidator;
            _productUpdateRequestValidator = productUpdateRequestValidator; 
            _mapper = mapper;
            _productsRepository = productsRepository;
        }
        public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
        {
            if(productAddRequest == null)
            {
                throw new ArgumentNullException(nameof(productAddRequest));
            }
            //validate the product using Fluent Validation
            ValidationResult validationResult = await _productAddRequestValidator.ValidateAsync(productAddRequest);

            if (!validationResult.IsValid)
            {
                string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
                throw new ArgumentException(errors);
            
            }
            //Attempt to add product
            Product productInput = _mapper.Map<Product>(productAddRequest); //Map productAddRequest into product type (it invokes ProductAddRequestToProductMappingProfile)
            Product? addedProduct = await _productsRepository.AddProduct(productInput);
            if (addedProduct == null)
            {
                return null;
            }

            ProductResponse addedProductResponse = _mapper.Map<ProductResponse>(addedProduct); //Map addedProduct into ProductResponse type (it invokes ProductToProductResponseMappingProfile)

            return addedProductResponse;
        }

        public async Task<bool> DeleteProduct(Guid productID)
        {
            Product? existingProduct = await _productsRepository.GetProductByCondition( temp=> temp.ProductID == productID);

            if (existingProduct == null)
            {
                return false;
            }

            //Attempt to delete product
            bool isDeleted = await _productsRepository.DeleteProduct(productID);
            return isDeleted;

        }

        public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
        {
            Product? product = await _productsRepository.GetProductByCondition( conditionExpression);
            if(product == null)
            {
                return null;
            }

            ProductResponse productResponse = _mapper.Map<ProductResponse>(product);// i nvoikes ProductToProductResponseMappingProfile

            return productResponse;
        }

        public async Task<List<ProductResponse>> GetProducts()
        {
            IEnumerable<Product?> products = await _productsRepository.GetProducts();

            IEnumerable<ProductResponse> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
            
            return productResponses.ToList();
        }

        public async Task<List<ProductResponse?>> GetProductNamesByCondition(string SearchString)
        {
            IEnumerable<Product?> products = await _productsRepository.GetProductNamesByCondition( SearchString);
            IEnumerable<ProductResponse> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
            
            return productResponses.ToList();
        }

        public async Task<List<ProductResponse?>> GetProductCategpriesByCondition(string SearchString)
        {
            IEnumerable<Product?> products = await _productsRepository.GetProductCategoriesByCondition(SearchString);
            IEnumerable<ProductResponse> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);

            return productResponses.ToList();
        }

        public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
        {
            Product? existingProduct = await _productsRepository.GetProductByCondition(temp=> temp.ProductID == productUpdateRequest.ProductID);

            if (existingProduct == null)
            {
                throw new ArgumentException("Invalid Product ID");
            }

            //Validate the product using FluentValidation
            ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);
            //Check the validation result
            if (!validationResult.IsValid)
            {
                string errors = string.Join(", ", validationResult.Errors.Select(temp =>temp.ErrorMessage));
            
                throw new ValidationException(errors);
            }

            //Map from ProductUpdateRequest to Product type
            Product product = _mapper.Map<Product>(productUpdateRequest);
            Product? updatedProduct = await _productsRepository.UpdateProduct(product);

            ProductResponse? updatedProductResponse = _mapper.Map<ProductResponse>(updatedProduct);
            
            return updatedProductResponse;
        
        }

        
    }
}
