using AutoMapper;
using MyTown.SharedModels.Features.Cards.Commands;

namespace MyTown.SharedModels.DTOs
    {

    //todo change all these later
    public class TownCardDto : TownCard
        {
        private class Mapping : Profile
            {
            public Mapping()
                {
                CreateMap<TownCard, TownCardDto>().ReverseMap();
                CreateMap<CreateUpdateTownCardCommand, TownCardDto>().ReverseMap();
                }
            }
        }
    }
