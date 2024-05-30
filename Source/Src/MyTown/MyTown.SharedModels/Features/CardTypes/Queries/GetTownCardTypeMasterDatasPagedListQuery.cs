namespace MyTown.SharedModels.Features.CardTypes.Queries
    {
    public class GetTownCardTypeMasterDatasPagedListQuery : PagenationRequestParameter, IRequest<PagedResponse<TownCardTypeDto>>
        {
        public bool All { get; set; }
        public string? Name { get; set; }
        }
    }
