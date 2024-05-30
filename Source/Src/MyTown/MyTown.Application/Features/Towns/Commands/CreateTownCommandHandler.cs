using MyTown.SharedModels.Features.Towns.Commands;

namespace MyTown.Application.Features.Towns.Commands
    {
    //not using this,instead using CreateUpdate
    public class CreateTownCommandHandler(ITownRepository townCardTypeMasteDataRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateTownCommand, BaseResult<int>>
        {
        public async Task<BaseResult<int>> Handle(CreateTownCommand request, CancellationToken cancellationToken)
            {
            var obj = request.To<CreateTownCommand, Town>();
            //todo should modify above 
            //var product = new Town(request.Name, request.Price, request.BarCode);

            await townCardTypeMasteDataRepository.AddAsync(obj);
            bool success = await unitOfWork.SaveChangesAsync();

            return new BaseResult<int>(obj.Id) { Success = success };
            }
        }
    }