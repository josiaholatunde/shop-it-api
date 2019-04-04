using System.Linq;
using ShoppingApp.API.DTOS;
using ShoppingApp.API.Models;

namespace ShoppingApp.API.Profile
{
    public class AutoMapperProfile: AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductToCreateDto, Product>();
            CreateMap<MerchantToCreateDto, Merchant>();
            CreateMap<StoreToCreateDto, Store>();
            CreateMap<BrandToCreateDto, Brand>();
            CreateMap<CategoryToCreateDto, Category>();


            // Domain to API Resource
            CreateMap<Product, ProductToReturnDto>()
            .ForMember(dest => dest.MerchantName, opt => opt.MapFrom(src => src.Merchant.Name))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName.Name));
            CreateMap<Feature, FeatureToReturn>();
            CreateMap<Brand, BrandToCreateDto>();
            CreateMap<Store, StoreToCreateDto>();
            CreateMap<BrandCategory, BrandToReturnDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Brand.Name));
        }
    }
}