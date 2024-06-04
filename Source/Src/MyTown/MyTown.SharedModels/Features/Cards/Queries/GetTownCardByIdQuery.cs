namespace MyTown.SharedModels.Features.Cards.Queries
    {
    public class GetTownCardByIdQuery : IRequest<BaseResult<TownCardDto>>
        {
        public int Id { get; set; }
        //dont pass userid over request,instead use authorizedclient & service api side use IAuthenticatedUser.Userid  or IsAuthenticated()
        }
    }
