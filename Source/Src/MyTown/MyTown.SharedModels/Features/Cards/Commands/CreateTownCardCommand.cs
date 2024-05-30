namespace MyTown.SharedModels.Features.Cards.Commands
    {
    public class CreateTownCardCommand : TownCard,//later should remove this domain type
        IRequest<BaseResult<int>>
        {
        //public int MyProperty { get; set; }
        }
    }