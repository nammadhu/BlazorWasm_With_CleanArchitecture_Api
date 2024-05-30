namespace MyTown.SharedModels.Features.Cards.Queries
    {
    public class GetTownCardByIdQuery : IRequest<BaseResult<TownCardDto>>
        {
        public int Id { get; set; }
        }
    }
