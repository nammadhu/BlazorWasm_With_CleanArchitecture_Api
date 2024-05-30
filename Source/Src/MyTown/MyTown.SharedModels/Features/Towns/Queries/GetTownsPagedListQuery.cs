namespace MyTown.SharedModels.Features.Towns.Queries
    {
    public class GetTownsPagedListQuery : PagenationRequestParameter, IRequest<PagedResponse<TownDto>>
        {
        public bool All { get; set; }
        public string? Name { get; set; }
        }
    }
