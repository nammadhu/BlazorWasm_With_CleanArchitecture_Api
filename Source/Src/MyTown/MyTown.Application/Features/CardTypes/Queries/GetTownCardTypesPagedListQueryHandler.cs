using MyTown.SharedModels.Features.CardTypes.Queries;
using SharedResponse.DTOs;

namespace MyTown.Application.Features.CardTypes.Queries
    {
    public class GetTownCardTypesPagedListQueryHandler(ITownCardTypeRepository townCardTypeMasteDataRepository) : IRequestHandler<GetTownCardTypesPagedListQuery, PagedResponse<TownCardTypeDto>>
        {
        public async Task<PagedResponse<TownCardTypeDto>> Handle(GetTownCardTypesPagedListQuery request, CancellationToken cancellationToken)
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