using MyTown.SharedModels.Features.CardTypes.Commands;

namespace MyTown.Application.Features.CardTypes.Commands
    {
    //not using this,instead using CreateUpdate
    public class CreateTownCardTypeCommandHandler(ITownCardTypeRepository townCardTypeMasteDataRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateTownCardTypeCommand, BaseResult<int>>
        {
        public async Task<BaseResult<int>> Handle(CreateTownCardTypeCommand request, CancellationToken cancellationToken)
            {
            var obj = request.To<CreateTownCardTypeCommand, TownCardType>();
            //todo should modify above 
            //var product = new TownCardType(request.Name, request.Price, request.BarCode);

            await townCardTypeMasteDataRepository.AddAsync(obj);
            bool success = await unitOfWork.SaveChangesAsync();

            return new BaseResult<int>(obj.Id) { Success = success };
            }
        }
    }