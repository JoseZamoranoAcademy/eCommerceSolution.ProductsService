using BusinessLogicLayer.Enumerations;
using Google.Protobuf.WellKnownTypes;

namespace BusinessLogicLayer.DTO
{
    public record ProductAddRequest(
        string ProductName,
        CategoryOptions Category,
        double? UnitPrice,
        int? QuantityInStock)
    {
        public ProductAddRequest() : this(default, default, default, default)
        {

        }
    }

    public class ProductAddRequest1
    {
        private CategoryOptions _categoryOptions;
        public string ProductName { get; set; }
        public string Category { get; set; }  
        public double? UnitPrice { get; set; }
        public int? QuantityInStock { get; set; }
    }

}
