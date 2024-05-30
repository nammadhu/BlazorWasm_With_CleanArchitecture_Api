using AutoMapper;
using MyTown.SharedModels.Features.Towns.Commands;

namespace MyTown.SharedModels.DTOs
    {

    //todo change all these later
    public class TownDto : Town
        {
        private class Mapping : Profile
            {
            public Mapping()
                {
                CreateMap<Town, TownDto>().ReverseMap();
                CreateMap<CreateUpdateTownCommand, TownDto>().ReverseMap();
                }
            }
        }
    }
