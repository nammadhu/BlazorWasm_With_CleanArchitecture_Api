namespace MyTown.SharedModels.Features.CardTypes.Queries
    {
    public class GetTownCardTypeMasterDataByIdQuery : IRequest<BaseResult<TownCardTypeDto>>
        {
        public int Id { get; set; }
        }
    }
