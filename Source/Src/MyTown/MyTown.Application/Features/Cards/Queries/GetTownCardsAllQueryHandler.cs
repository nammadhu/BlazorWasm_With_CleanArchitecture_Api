using MyTown.SharedModels.Features.Cards.Queries;

namespace MyTown.Application.Features.Cards.Queries
    {
    public class GetTownCardsAllQueryHandler(ITownCardRepository townCardRepo) : IRequestHandler<GetTownCardsAllQuery, IReadOnlyList<TownCardDto>>
        {
        public async Task<IReadOnlyList<TownCardDto>> Handle(GetTownCardsAllQuery request, CancellationToken cancellationToken)
            {
            var res = await townCardRepo.GetAllAsync();

            return res.Select(r => r.To<TownCard, TownCardDto>()).ToList();
            }
        }
    }