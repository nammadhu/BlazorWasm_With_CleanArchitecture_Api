namespace MyTown.SharedModels.Features.Cards.Queries
    {
    public class GetTownCardsQueryOfTownId : IRequest<IReadOnlyList<TownCardDto>>
        {
        public int TownId { get; set; }
    }
    }
