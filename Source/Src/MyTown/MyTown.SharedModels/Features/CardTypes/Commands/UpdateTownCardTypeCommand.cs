using AutoMapper;

namespace MyTown.SharedModels.Features.CardTypes.Commands
    {
    //not using this,instead using CreateUpdate
    public class UpdateTownCardTypeCommand : TownCardType,//later should remove this domain type
        IRequest<BaseResult>
        {
        //public int MyProperty { get; set; }

        private class Mapping : Profile
            {
            public Mapping()
                {
                CreateMap<TownCardType, UpdateTownCardTypeCommand>().ReverseMap();
                }
            }

        }
    }