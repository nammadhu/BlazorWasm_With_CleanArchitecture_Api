using AutoMapper;
using MyTown.SharedModels.Features.CardTypes.Commands;

namespace MyTown.SharedModels.DTOs
    {

    //todo change all these later
    public class TownCardTypeDto : TownCardType
        {
        private class Mapping : Profile
            {
            public Mapping()
                {
                CreateMap<TownCardType, TownCardTypeDto>().ReverseMap();
                CreateMap<CreateUpdateTownCardTypeCommand, TownCardTypeDto>().ReverseMap();
                }
            }
        }
    }
