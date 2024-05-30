namespace MyTown.SharedModels.Features.Cards.Queries
    {
    public class GetTownCardsPagedListQuery : PagenationRequestParameter, IRequest<PagedResponse<TownCardDto>>
        {
        public bool All { get; set; }
        public string? Name { get; set; }
        }
    }
