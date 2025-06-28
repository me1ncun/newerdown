using AutoMapper;
using NewerDown.Domain.DTOs.Notifications;
using NewerDown.Domain.Entities;

namespace NewerDown.Application.MappingProfiles;

public class NotificationRuleMappingProfile : Profile
{
    public NotificationRuleMappingProfile()
    {
        CreateMap<AddNotificationRuleDto, NotificationRule>();

        CreateMap<NotificationRule, NotificationRuleDto>();
    }
}