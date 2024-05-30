using AutoMapper;

namespace MyTown.SharedModels.Features.Cards.Commands
    {
    public class UpdateTownCardCommand : TownCard,//later should remove this domain type
        IRequest<BaseResult>
        {
        //public int MyProperty { get; set; }

        private class Mapping : Profile
            {
            public Mapping()
                {
                CreateMap<TownCard, UpdateTownCardCommand>().ReverseMap();
                }
            }

        }
    }