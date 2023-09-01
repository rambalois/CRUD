using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles
{
    public class CSGOMappings : Profile
    {
        public CSGOMappings()
        {
            CreateMap<CSGOEntity, CSGODto>().ReverseMap();
            CreateMap<CSGOEntity, CSGOUpdateDto>().ReverseMap();
            CreateMap<CSGOEntity, CSGOCreateDto>().ReverseMap();
        }
    }
}
