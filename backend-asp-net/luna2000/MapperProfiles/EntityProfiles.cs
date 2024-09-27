using AutoMapper;
using luna2000.Dto;
using luna2000.Models;

namespace luna2000.MapperProfiles;

public class EntityProfiles : Profile
{
    public EntityProfiles()
    {
        CreateMap<AddCarRequest, CarEntity>()
            .ForMember(entity => entity.Photos, expression => expression.Ignore())
            .ForMember(entity => entity.Id, expression => expression.Ignore());
        CreateMap<IFormFile, PhotoEntity>()
            .ForMember(entity => entity.FileName,
                expression => expression.MapFrom(file => file.FileName))
            .ForMember(entity => entity.FileExtension,
                expression => expression.MapFrom(file => Path.GetExtension(file.FileName)));
        CreateMap<AddDriverRequest, DriverEntity>()
            .ForMember(entity => entity.Photos, expression => expression.Ignore())
            .ForMember(entity => entity.Id, expression => expression.Ignore());
    }
}