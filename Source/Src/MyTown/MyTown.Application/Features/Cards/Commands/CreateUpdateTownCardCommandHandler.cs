using MyTown.SharedModels.Features.Cards.Commands;
using SharedResponse;

namespace MyTown.Application.Features.Cards.Commands
    {
    public class CreateUpdateTownCardCommandHandler(ITownCardRepository repository, IUnitOfWork unitOfWork,
        ITranslator translator, IMapper mapper) : IRequestHandler<CreateUpdateTownCardCommand, BaseResult<TownCardDto>>
        {
        public async Task<BaseResult<TownCardDto>> Handle(CreateUpdateTownCardCommand request, CancellationToken cancellationToken)
            {
            if (request.Id > 0)//UPDATE
                {
                var existingData = await repository.GetByIdIntAsync(request.Id);
                if (existingData is null)
                    {
                    return new BaseResult<TownCardDto>()
                        {
                        Success = false,
                        Errors = [new(ErrorCode.NotFound, translator.GetString(TranslatorMessages.ProductMessages.Product_notfound_with_id(request.Id)), nameof(request.Id))]
                        };
                    }
                var isDuplicateResult = await NameExists(repository, translator, request, existingData);
                if (isDuplicateResult != null) return isDuplicateResult;

                //todo must modify all specific properties here with validation on validator
                existingData = mapper.Map(request, existingData);
                existingData.Id = request.Id;

                repository.Update(existingData);

                return new BaseResult<TownCardDto>() { Success = await unitOfWork.SaveChangesAsync(), Data = mapper.Map<TownCardDto>(existingData) };
                }
            else//create
                {
                var isDuplicateResult = await NameExists(repository, translator, request, null);
                if (isDuplicateResult != null) return isDuplicateResult;

                var obj = request.To<CreateUpdateTownCardCommand, TownCard>();
                //todo should modify above 
                //var product = new TownCard(request.Name, request.Price, request.BarCode);



                var result = await repository.AddAsync(obj);
                //var success = await unitOfWork.SaveChangesAsync();
                return new BaseResult<TownCardDto>() { Success = await unitOfWork.SaveChangesAsync(), Data = mapper.Map<TownCardDto>(result) };
                }
            }

        private static async Task<BaseResult<TownCardDto>?> NameExists(ITownCardRepository repository, ITranslator translator, CreateUpdateTownCardCommand request, TownCard? existingData)
            {
            if (existingData is null && request.Id > 0)//exisitng shuld have matching record
                {
                return new BaseResult<TownCardDto>()
                    {
                    Success = false,
                    Errors = [new(ErrorCode.ModelIsNull, translator.GetString($"Data Mapping Is Null"), nameof(request.Id))]
                    };
                }
            if ((existingData == null || !request.Name.Equals(existingData.Name, StringComparison.CurrentCultureIgnoreCase))
                && await repository.IsNameExistsAsync(request.Name))//if different then check other shouldnot exists
                {
                return new BaseResult<TownCardDto>()
                    {
                    Success = false,
                    Errors = [new(ErrorCode.DuplicateData, translator.GetString($"Name({request.Name}) Already exists"), nameof(request.Name))]
                    };
                }
            return null;
            }
        }
    }