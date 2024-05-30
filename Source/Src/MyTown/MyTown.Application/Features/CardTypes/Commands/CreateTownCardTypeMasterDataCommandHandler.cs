using MyTown.SharedModels.Features.CardTypes.Commands;

namespace MyTown.Application.Features.CardTypes.Commands
    {
    //not using this,instead using CreateUpdate
    public class CreateTownCardTypeMasterDataCommandHandler(ITownCardTypeMasterDataRepository townCardTypeMasteDataRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateTownCardTypeMasterDataCommand, BaseResult<int>>
        {
        public async Task<BaseResult<int>> Handle(CreateTownCardTypeMasterDataCommand request, CancellationToken cancellationToken)
            {
            var obj = request.To<CreateTownCardTypeMasterDataCommand, TownCardTypeMasterData>();
            //todo should modify above 
            //var product = new TownCardTypeMasterData(request.Name, request.Price, request.BarCode);

            await townCardTypeMasteDataRepository.AddAsync(obj);
            bool success = await unitOfWork.SaveChangesAsync();

            return new BaseResult<int>(obj.Id) { Success = success };
            }
        }
    }