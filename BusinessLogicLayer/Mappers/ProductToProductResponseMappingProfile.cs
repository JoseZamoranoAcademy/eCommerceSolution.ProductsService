using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Enumerations;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Mappers
{
    internal class ProductToProductResponseMappingProfile : Profile
    {

        public ProductToProductResponseMappingProfile()
        {
            CategoryOptions myStatus;// = CategoryOptions.None;
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
               // .ForMember(dest => dest.Category, opt => opt.MapFrom<CategoryOptions> (src => Enum.TryParse( src.Category, out myStatus)))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock))
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID));
        }
    }
}
