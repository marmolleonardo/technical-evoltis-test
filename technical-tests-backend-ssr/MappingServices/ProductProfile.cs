using AutoMapper;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.MappingServices
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Entity -> Read DTO
            CreateMap<Product, ProductReadDto>();

            // Create DTO -> Entity
            CreateMap<ProductCreateDto, Product>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore()) // handled by DB default
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());

            // Update DTO -> Entity (partial update handled in service)
            CreateMap<ProductUpdateDto, Product>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore());
        }
    }
}
