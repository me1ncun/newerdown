using AutoMapper;
using NewerDown.Domain.Entities;

namespace NewerDown.Application.MappingProfiles;

public class FileAttachmentMappingProfile : Profile
{
    public FileAttachmentMappingProfile()
    {
        CreateMap<Domain.DTOs.File.FileAttachmentDto, FileAttachment>()
            .ForMember(des => des.Uri, opt => opt.MapFrom(src => src.Uri));
    }
}