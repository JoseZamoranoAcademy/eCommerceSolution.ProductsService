using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Enumerations;
using BusinessLogicLayer.ServiceContracts;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;


namespace ProductsMicroService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IValidator<ProductAddRequest> _addProductRequestValidator;
        private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;

        public ProductsController(IProductService productService , IValidator<ProductAddRequest> addProductRequestValidator , IValidator<ProductUpdateRequest> productUpdateRequestValidator)
        {
            _productService = productService;
            _addProductRequestValidator = addProductRequestValidator;
            _productUpdateRequestValidator = productUpdateRequestValidator;
        }


        [Route("")] //api/products
        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            List<ProductResponse?> products = await _productService.GetProducts();
            return Ok(products);
        }

        [Route("search/product-id/{ProductID:guid}")] ////GET /api/products/search/product-id/00000000-0000-0000-0000-000000000000
        [HttpGet]
        public async Task<IActionResult> GetProductByIDAsync( Guid ProductID)
        {
            ProductResponse? product = await _productService.GetProductByCondition(temp => temp.ProductID == ProductID);
            
            return Ok(product);
        }

        [Route("/api/products/search/{SearchString}")] //GET /api/products/search/xxxxxxxxxxxxxxxxxx
        [HttpGet]
        public async Task<IActionResult> SearchProductsByConditionAsync(string SearchString)
        {
            List<ProductResponse> productsByProductName = await _productService.GetProductNamesByCondition(SearchString);


            List<ProductResponse> productsByCategory = await _productService.GetProductCategpriesByCondition(SearchString);
        
            var products = productsByProductName.Union(productsByCategory);
        
            return Ok(products);
        }


        [Route("")] //POST /api/products
        [HttpPost]
        public async Task<IActionResult> AddProductAsync(ProductAddRequest1 productAddRequest1)
        {
            CategoryOptions optionsBase;

            var parseIsSuccess = Enum.TryParse(productAddRequest1.Category, out optionsBase);
            ProductAddRequest prodAddRequest = new ProductAddRequest(productAddRequest1.ProductName, optionsBase, productAddRequest1.UnitPrice, productAddRequest1.QuantityInStock);

            ValidationResult validationResult = await _addProductRequestValidator.ValidateAsync(prodAddRequest);

            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                    .GroupBy(temp => temp.PropertyName)
                    .ToDictionary(group => group.Key, group => group.Select(err => err.ErrorMessage).ToArray());

                var info = Results.ValidationProblem(errors);

                return BadRequest(info);
            }
            var addedProductResponse = await _productService.AddProduct(prodAddRequest);
            if (addedProductResponse != null) 
            {
                return Ok(addedProductResponse);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Error in adding product");
            }        
        }


        [Route("")] //PUT /api/products
        [HttpPut]
        public async Task<IActionResult> UpdateProductAsync(ProductUpdateRequest1 productUpdateRequest1)
        {
            CategoryOptions optionsBase;

            var parseIsSuccess = Enum.TryParse(productUpdateRequest1.Category, out optionsBase);
            ProductUpdateRequest prodUpdateRequest = new ProductUpdateRequest(
                productUpdateRequest1.ProductID,
                productUpdateRequest1.ProductName,
                optionsBase, 
                productUpdateRequest1.UnitPrice,
                productUpdateRequest1.QuantityInStock);

            ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(prodUpdateRequest);

            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                    .GroupBy(temp => temp.PropertyName)
                    .ToDictionary(group => group.Key, grp => grp.Select(err => err.ErrorMessage).ToArray());

                var info = Results.ValidationProblem(errors);
                return BadRequest(info);
            }

            var updatedProductResponse = await _productService.UpdateProduct(prodUpdateRequest);
            if (updatedProductResponse != null)
            {
                return Ok(updatedProductResponse);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in updating product");
            }
        
        }

        [Route("{ProductID:guid}")] //DELETE /api/products/xxxxxxxxxxxxxxxxxxx
        [HttpDelete]
        public async Task<IActionResult> DeleteProductAsync(Guid ProductID)
        {
            bool isDeleted = await _productService.DeleteProduct(ProductID);
            if (isDeleted)
            {
                return Ok(isDeleted);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in deleting product");
            }
        }
    }
}
