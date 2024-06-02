namespace MyTown.SharedModels.Features.CardTypes.Queries
    {
    public class GetTownCardTypeByIdQuery : IRequest<BaseResult<TownCardTypeDto>>
        {
        public int Id { get; set; }
        }
    }
