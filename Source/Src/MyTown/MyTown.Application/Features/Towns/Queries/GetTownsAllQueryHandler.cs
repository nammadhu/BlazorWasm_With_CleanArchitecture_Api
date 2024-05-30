using MyTown.SharedModels.Features.Towns.Queries;
using SharedResponse.DTOs;

namespace MyTown.Application.Features.Towns.Queries
    {
    public class GetTownsAllQueryHandler(ITownRepository TownMasteDataRepository) : IRequestHandler<GetTownsAllQuery, IReadOnlyList<TownDto>>
        {
        public async Task<IReadOnlyList<TownDto>> Handle(GetTownsAllQuery request, CancellationToken cancellationToken)
            {
            var res = await TownMasteDataRepository.GetAllAsync();

            return res.Select(r => r.To<Town, TownDto>()).ToList();
            }
        }
    }