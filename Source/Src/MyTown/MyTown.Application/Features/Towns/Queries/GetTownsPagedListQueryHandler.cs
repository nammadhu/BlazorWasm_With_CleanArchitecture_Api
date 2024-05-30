using MyTown.SharedModels.Features.Towns.Queries;
using SharedResponse.DTOs;

namespace MyTown.Application.Features.Towns.Queries
    {
    public class GetTownsPagedListQueryHandler(ITownRepository TownMasteDataRepository) : IRequestHandler<GetTownsPagedListQuery, PagedResponse<TownDto>>
        {
        public async Task<PagedResponse<TownDto>> Handle(GetTownsPagedListQuery request, CancellationToken cancellationToken)
            {
            PagenationResponseDto<TownDto> result;
            if (request.All)
                result = await TownMasteDataRepository.GetPagedListAsync(1, int.MaxValue, request.Name);
            else
                result = await TownMasteDataRepository.GetPagedListAsync(request.PageNumber, request.PageSize, request.Name);

            return new PagedResponse<TownDto>(result);
            }
        }
    }