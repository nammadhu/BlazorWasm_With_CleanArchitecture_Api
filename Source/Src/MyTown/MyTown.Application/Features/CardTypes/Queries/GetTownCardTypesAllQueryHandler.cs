using MyTown.SharedModels.Features.CardTypes.Queries;

namespace MyTown.Application.Features.CardTypes.Queries
    {
    public class GetTownCardTypeMasterDatasAllQueryHandler(ITownCardTypeMasterDataRepository cardTypeRepository) : IRequestHandler<GetTownCardTypeMasterDatasAllQuery, IReadOnlyList<TownCardTypeDto>>
        {
        public async Task<IReadOnlyList<TownCardTypeDto>> Handle(GetTownCardTypeMasterDatasAllQuery request, CancellationToken cancellationToken)
            {
            var res = await cardTypeRepository.GetAllAsync();

            return res.Select(r => r.To<TownCardTypeMasterData, TownCardTypeDto>()).ToList();
            }
        }
    }