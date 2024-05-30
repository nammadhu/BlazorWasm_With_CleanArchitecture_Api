namespace MyTown.SharedModels.Features.Towns.Commands
    {
    public class CreateUpdateTownCommand : Town, IRequest<BaseResult<TownDto>>
        {
        }
    }