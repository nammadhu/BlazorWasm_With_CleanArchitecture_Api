namespace MyTown.SharedModels.Features.CardTypes.Commands
    {
    //not using this,instead using CreateUpdate
    public class CreateTownCardTypeMasterDataCommand : TownCardTypeMasterData,//later should remove this domain type
        IRequest<BaseResult<int>>
        {
        //public int MyProperty { get; set; }
        }
    }