using MyTown.SharedModels.Features.Cards.Queries;

namespace MyTown.Application.Features.Cards.Queries
    {
    public class GetUserCardsMoreDetailsHandler(ITownCardRepository townCardRepo) : IRequestHandler<GetUserCardsMoreDetails, (List<int> approvedCardIds, List<TownCard> draftCards)>
        {
        public async Task<(List<int> approvedCardIds, List<TownCard> draftCards)> Handle(GetUserCardsMoreDetails request, CancellationToken cancellationToken)
            {

            return await townCardRepo.GetUserCardsMoreDetails(request.UserId, request.IsCardCreator, request.IsCardOwner, request.IsCardApprovedOwner, request.IsCardApprovedReviewer, request.TownId);
            }
        }
    }