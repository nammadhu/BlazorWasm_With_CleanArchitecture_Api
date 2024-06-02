using MyTown.SharedModels.Features.CardTypes.Queries;
using SharedResponse;

namespace MyTown.Application.Features.CardTypes.Queries
    {
    public class GetTownCardTypeByIdQueryHandler(ITownCardTypeRepository townCardTypeRepository, ITranslator translator) : IRequestHandler<GetTownCardTypeByIdQuery, BaseResult<TownCardTypeDto>>
        {
        public async Task<BaseResult<TownCardTypeDto>> Handle(GetTownCardTypeByIdQuery request, CancellationToken cancellationToken)
            {
            var product = await townCardTypeRepository.GetByIdIntAsync(request.Id);

            if (product is null)
                {
                return new BaseResult<TownCardTypeDto>(new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.ProductMessages.Product_notfound_with_id(request.Id)), nameof(request.Id)));
                }

            var result = product.To<TownCardType, TownCardTypeDto>();

            return new BaseResult<TownCardTypeDto>(result);
            }
        }
    }
