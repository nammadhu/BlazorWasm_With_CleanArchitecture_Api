using MyTown.SharedModels.Features.Towns.Queries;
using SharedResponse;

namespace MyTown.Application.Features.Towns.Queries
    {
    public class GetTownByIdQueryHandler(ITownRepository TownRepository, ITranslator translator) : IRequestHandler<GetTownByIdQuery, BaseResult<TownDto>>
        {
        public async Task<BaseResult<TownDto>> Handle(GetTownByIdQuery request, CancellationToken cancellationToken)
            {
            var town = await TownRepository.GetByIdIntAsync(request.Id,request.UserId);

            if (town is null)
                {
                return new BaseResult<TownDto>(new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.ProductMessages.Product_notfound_with_id(request.Id)), nameof(request.Id)));
                }

            var result = town.To<Town, TownDto>();

            return new BaseResult<TownDto>(result);
            }
        }
    }
