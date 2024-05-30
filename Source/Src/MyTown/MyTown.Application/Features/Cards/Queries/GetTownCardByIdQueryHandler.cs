using MyTown.SharedModels.Features.Cards.Queries;
using SharedResponse;

namespace MyTown.Application.Features.Cards.Queries
    {
    public class GetTownCardByIdQueryHandler(ITownCardRepository TownCardRepository, ITranslator translator) : IRequestHandler<GetTownCardByIdQuery, BaseResult<TownCardDto>>
        {
        public async Task<BaseResult<TownCardDto>> Handle(GetTownCardByIdQuery request, CancellationToken cancellationToken)
            {
            var product = await TownCardRepository.GetByIdIntAsync(request.Id);

            if (product is null)
                {
                return new BaseResult<TownCardDto>(new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.ProductMessages.Product_notfound_with_id(request.Id)), nameof(request.Id)));
                }

            var result = product.To<TownCard, TownCardDto>();

            return new BaseResult<TownCardDto>(result);
            }
        }
    }
