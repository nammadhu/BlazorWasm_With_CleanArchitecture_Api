using MyTown.SharedModels.Features.CardTypes.Queries;
using SharedResponse;

namespace MyTown.Application.Features.CardTypes.Queries
    {
    public class GetTownCardTypeMasterDataByIdQueryHandler(ITownCardTypeMasterDataRepository townCardTypeMasterDataRepository, ITranslator translator) : IRequestHandler<GetTownCardTypeMasterDataByIdQuery, BaseResult<TownCardTypeDto>>
        {
        public async Task<BaseResult<TownCardTypeDto>> Handle(GetTownCardTypeMasterDataByIdQuery request, CancellationToken cancellationToken)
            {
            var product = await townCardTypeMasterDataRepository.GetByIdIntAsync(request.Id);

            if (product is null)
                {
                return new BaseResult<TownCardTypeDto>(new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.ProductMessages.Product_notfound_with_id(request.Id)), nameof(request.Id)));
                }

            var result = product.To<TownCardTypeMasterData, TownCardTypeDto>();

            return new BaseResult<TownCardTypeDto>(result);
            }
        }
    }
