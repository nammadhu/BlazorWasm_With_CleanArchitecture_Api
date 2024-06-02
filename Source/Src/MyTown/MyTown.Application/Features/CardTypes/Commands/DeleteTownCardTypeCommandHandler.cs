using MyTown.SharedModels.Features.CardTypes.Commands;
using SharedResponse;

namespace MyTown.Application.Features.CardTypes.Commands
    {
    public class DeleteTownCardTypeCommandHandler(ITownCardTypeRepository townCardTypeMasteDataRepository, IUnitOfWork unitOfWork, ITranslator translator) : IRequestHandler<DeleteTownCardTypeCommand, BaseResult>
        {
        public async Task<BaseResult> Handle(DeleteTownCardTypeCommand request, CancellationToken cancellationToken)
            {
            var data = await townCardTypeMasteDataRepository.GetByIdIntAsync(request.Id);

            if (data is null)
                {
                return new BaseResult(new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.ProductMessages.Product_notfound_with_id(request.Id)), nameof(request.Id)));
                }

            townCardTypeMasteDataRepository.Delete(data);
            await unitOfWork.SaveChangesAsync();

            return new BaseResult();
            }
        }
    }
