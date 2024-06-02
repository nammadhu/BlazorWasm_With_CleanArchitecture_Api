using MyTown.SharedModels.Features.CardTypes.Queries;

namespace MyTown.Application.Features.CardTypes.Queries
    {
    public class GetTownCardTypesAllQueryHandler(ITownCardTypeRepository cardTypeRepository) : IRequestHandler<GetTownCardTypesAllQuery, IReadOnlyList<TownCardTypeDto>>
        {
        public async Task<IReadOnlyList<TownCardTypeDto>> Handle(GetTownCardTypesAllQuery request, CancellationToken cancellationToken)
            {
            var res = await cardTypeRepository.GetAllAsync();

            return res.Select(r => r.To<TownCardType, TownCardTypeDto>()).ToList();
            }
        }
    }