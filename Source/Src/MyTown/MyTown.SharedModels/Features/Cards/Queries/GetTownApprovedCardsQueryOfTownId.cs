namespace MyTown.SharedModels.Features.Cards.Queries
    {
    public class GetTownApprovedCardsQueryOfTownId : IRequest<IReadOnlyList<TownApprovedCardDto>>
        {
        public int TownId { get; set; }
        }
    }
