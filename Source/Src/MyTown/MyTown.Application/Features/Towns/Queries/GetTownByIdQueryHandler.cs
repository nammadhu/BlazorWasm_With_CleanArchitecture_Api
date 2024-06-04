using MyTown.SharedModels.Features.Towns.Queries;
using SharedResponse;

namespace MyTown.Application.Features.Towns.Queries
    {
    public class GetTownByIdQueryHandler(ITownRepository TownRepository, ITranslator translator//, IAuthenticatedUserService authenticatedUser
        ) : IRequestHandler<GetTownByIdQuery, BaseResult<TownDto>>
        {
        public async Task<BaseResult<TownDto>> Handle(GetTownByIdQuery request, CancellationToken cancellationToken)
            {
            //Console.WriteLine("Userid isssss");
            //Console.WriteLine(authenticatedUser.UserId);

            //Town town;
            //if (authenticatedUser?.UserId != null && Guid.TryParse(authenticatedUser.UserId, out Guid userGuid))
            //    town = await TownRepository.GetByIdIntAsync(request.Id, userGuid);
            //else
            var town = await TownRepository.GetByIdIntAsync(request.Id);

            if (town is null)
                {
                return new BaseResult<TownDto>(new Error(ErrorCode.NotFound, $"Town({request.Id}) Details not found()", nameof(request.Id)));
                }

            var result = town.To<Town, TownDto>();

            return new BaseResult<TownDto>(result);
            }
        }
    }
