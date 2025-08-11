using BusinessLogicLayer.Enumerations;

namespace BusinessLogicLayer.DTO
{
    public record ProductUpdateRequest(Guid ProductID, string ProductName, CategoryOptions Category, double? UnitPrice , int? QuantityInStock)
    {
        public ProductUpdateRequest() :this(default, default, default, default, default)
        {
            
        }
    }

    public class ProductUpdateRequest1
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public double? UnitPrice { get; set; }
        public int? QuantityInStock { get; set; }
    }
}
