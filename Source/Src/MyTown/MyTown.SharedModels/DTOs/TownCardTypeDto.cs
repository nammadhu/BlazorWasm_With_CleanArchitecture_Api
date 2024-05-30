using AutoMapper;
using MyTown.SharedModels.Features.CardTypes.Commands;

namespace MyTown.SharedModels.DTOs
    {

    //todo change all these later
    public class TownCardTypeDto : TownCardTypeMasterData
        {
        private class Mapping : Profile
            {
            public Mapping()
                {
                CreateMap<TownCardTypeMasterData, TownCardTypeDto>().ReverseMap();
                CreateMap<CreateUpdateTownCardTypeMasterDataCommand, TownCardTypeDto>().ReverseMap();
                }
            }
        }
    }
