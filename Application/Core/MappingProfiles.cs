using System;
using Application.Activities.DTO;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Activity, Activity>();

        CreateMap<BaseActivityDTO, Activity>();

        CreateMap<CreateActivityDto, Activity>()
            .IncludeBase<BaseActivityDTO, Activity>();

        CreateMap<EditActivityDto, Activity>()
            .IncludeBase<BaseActivityDTO, Activity>();
    }
}
