using MyTown.SharedModels.Features.Towns.Queries;
using SharedResponse;

namespace MyTown.Application.Features.Towns.Queries
    {
    public class GetTownByIdQueryHandler(ITownRepository TownRepository, ITranslator translator) : IRequestHandler<GetTownByIdQuery, BaseResult<TownDto>>
        {
        public async Task<BaseResult<TownDto>> Handle(GetTownByIdQuery request, CancellationToken cancellationToken)
            {
            var product = await TownRepository.GetByIdIntAsync(request.Id);

            if (product is null)
                {
                return new BaseResult<TownDto>(new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.ProductMessages.Product_notfound_with_id(request.Id)), nameof(request.Id)));
                }

            var result = product.To<Town, TownDto>();

            return new BaseResult<TownDto>(result);
            }
        }
    }
