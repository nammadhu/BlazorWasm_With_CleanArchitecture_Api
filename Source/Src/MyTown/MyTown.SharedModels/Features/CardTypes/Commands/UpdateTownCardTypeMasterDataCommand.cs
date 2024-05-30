using AutoMapper;

namespace MyTown.SharedModels.Features.CardTypes.Commands
    {
    //not using this,instead using CreateUpdate
    public class UpdateTownCardTypeMasterDataCommand : TownCardTypeMasterData,//later should remove this domain type
        IRequest<BaseResult>
        {
        //public int MyProperty { get; set; }

        private class Mapping : Profile
            {
            public Mapping()
                {
                CreateMap<TownCardTypeMasterData, UpdateTownCardTypeMasterDataCommand>().ReverseMap();
                }
            }

        }
    }