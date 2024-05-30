using MyTown.SharedModels.Features.CardTypes.Queries;
using SharedResponse.DTOs;

namespace MyTown.Application.Features.CardTypes.Queries
    {
    public class GetTownCardTypeMasterDatasPagedListQueryHandler(ITownCardTypeMasterDataRepository townCardTypeMasteDataRepository) : IRequestHandler<GetTownCardTypeMasterDatasPagedListQuery, PagedResponse<TownCardTypeDto>>
        {
        public async Task<PagedResponse<TownCardTypeDto>> Handle(GetTownCardTypeMasterDatasPagedListQuery request, CancellationToken cancellationToken)
            {
            PagenationResponseDto<TownCardTypeDto> result;
            if (request.All)
                result = await townCardTypeMasteDataRepository.GetPagedListAsync(1, int.MaxValue, request.Name);
            else
                result = await townCardTypeMasteDataRepository.GetPagedListAsync(request.PageNumber, request.PageSize, request.Name);

            return new PagedResponse<TownCardTypeDto>(result);
            }
        }
    }