using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class ProductMappingProfiles : Profile
    {
        public ProductMappingProfiles()
        {
            CreateMap<ProductDto, Product>()
            .ForMember(dest => dest.ProductBrandId, opt => opt.MapFrom(src => src.ProductBrandId)) // Map ProductBrandId
            .ForMember(dest => dest.ProductTypeId, opt => opt.MapFrom(src => src.ProductTypeId))   // Map ProductTypeId
            .ForMember(dest => dest.ProductBrand, opt => opt.Ignore()) // Ignore complex object mapping
            .ForMember(dest => dest.ProductType, opt => opt.Ignore()); // Ignore complex object mapping
        }
    }
}