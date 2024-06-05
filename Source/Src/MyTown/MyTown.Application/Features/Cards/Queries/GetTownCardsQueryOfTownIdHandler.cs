using MyTown.SharedModels.Features.Cards.Queries;

namespace MyTown.Application.Features.Cards.Queries
    {
    public class GetTownCardsQueryOfTownIdHandler(ITownCardRepository townCardRepo) : IRequestHandler<GetTownCardsQueryOfTownId, IReadOnlyList<TownCardDto>>
        {
        public async Task<IReadOnlyList<TownCardDto>> Handle(GetTownCardsQueryOfTownId request, CancellationToken cancellationToken)
            {
            
            throw new NotImplementedException();
            //var res = await townCardRepo.GetAllAsync();

            //return res.Select(r => r.To<TownCard, TownCardDto>()).ToList();
            }
        }
    }