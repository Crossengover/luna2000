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
            .ForMember(entity => entity.Id, expression => expression.Ignore())
            .ForMember(entity => entity.RegistrationDate,
                expression => expression.MapFrom(request => DateOnly.FromDateTime(request.RegistrationDate)))
            .ForMember(entity => entity.TechInspection,
                expression => expression.MapFrom(request => DateOnly.FromDateTime(request.TechInspection)));
        CreateMap<IFormFile, PhotoEntity>()
            .ForMember(entity => entity.FileName,
                expression => expression.MapFrom(file => file.FileName))
            .ForMember(entity => entity.FileExtension,
                expression => expression.MapFrom(file => Path.GetExtension(file.FileName)));
        CreateMap<AddDriverRequest, DriverEntity>()
            .ForMember(entity => entity.Photos, expression => expression.Ignore())
            .ForMember(entity => entity.Id, expression => expression.Ignore());
        CreateMap<BaseLog, HistoryDto>();
    }
}