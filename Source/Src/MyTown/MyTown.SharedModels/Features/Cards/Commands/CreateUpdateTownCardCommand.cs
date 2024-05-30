namespace MyTown.SharedModels.Features.Cards.Commands
    {
    public class CreateUpdateTownCardCommand : TownCard, IRequest<BaseResult<TownCardDto>>
        {
        }
    }