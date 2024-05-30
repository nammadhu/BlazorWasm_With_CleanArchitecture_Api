using MyTown.SharedModels.Features.Cards.Queries;
using SharedResponse.DTOs;

namespace MyTown.Application.Features.Cards.Queries
    {
    public class GetTownCardsPagedListQueryHandler(ITownCardRepository TownCardMasteDataRepository) : IRequestHandler<GetTownCardsPagedListQuery, PagedResponse<TownCardDto>>
        {
        public async Task<PagedResponse<TownCardDto>> Handle(GetTownCardsPagedListQuery request, CancellationToken cancellationToken)
            {
            PagenationResponseDto<TownCardDto> result;
            if (request.All)
                result = await TownCardMasteDataRepository.GetPagedListAsync(1, int.MaxValue, request.Name);
            else
                result = await TownCardMasteDataRepository.GetPagedListAsync(request.PageNumber, request.PageSize, request.Name);

            return new PagedResponse<TownCardDto>(result);
            }
        }
    }