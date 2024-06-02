using MyTown.Application.Interfaces;
using MyTown.SharedModels.Features.CardTypes.Commands;
using SharedResponse;

namespace MyTown.Application.Features.CardTypes.Commands
    {
    public class CreateUpdateTownCardTypeMasterDataCommandHandler(ITownCardTypeMasterDataRepository repository, IUnitOfWork unitOfWork,
        ITranslator translator, IMapper mapper, IIDGenerator<Town> idNextGenerator) : IRequestHandler<CreateUpdateTownCardTypeMasterDataCommand, BaseResult<TownCardTypeDto>>
        {
        public async Task<BaseResult<TownCardTypeDto>> Handle(CreateUpdateTownCardTypeMasterDataCommand request, CancellationToken cancellationToken)
            {
            if (request.Id > 0)//UPDATE
                {
                var existingData = await repository.GetByIdIntAsync(request.Id);
                if (existingData is null)
                    {
                    return new BaseResult<TownCardTypeDto>()
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

                return new BaseResult<TownCardTypeDto>() { Success = await unitOfWork.SaveChangesAsync(), Data = mapper.Map<TownCardTypeDto>(existingData) };
                }
            else//create
                {
                var isDuplicateResult = await NameExists(repository, translator, request, null);
                if (isDuplicateResult != null) return isDuplicateResult;

                var obj = request.To<CreateUpdateTownCardTypeMasterDataCommand, TownCardTypeMasterData>();
                //todo should modify above 
                //var product = new TownCardTypeMasterData(request.Name, request.Price, request.BarCode);


                obj.Id = idNextGenerator.GetNextID();
                var result = await repository.AddAsync(obj);
                //var success = await unitOfWork.SaveChangesAsync();
                return new BaseResult<TownCardTypeDto>() { Success = await unitOfWork.SaveChangesAsync(), Data = mapper.Map<TownCardTypeDto>(result) };
                }
            }

        private static async Task<BaseResult<TownCardTypeDto>?> NameExists(ITownCardTypeMasterDataRepository repository, ITranslator translator, CreateUpdateTownCardTypeMasterDataCommand request, TownCardTypeMasterData? existingData)
            {
            if (existingData is null && request.Id > 0)//exisitng shuld have matching record
                {
                return new BaseResult<TownCardTypeDto>()
                    {
                    Success = false,
                    Errors = [new(ErrorCode.ModelIsNull, translator.GetString($"Data Mapping Is Null"), nameof(request.Id))]
                    };
                }
            if ((existingData == null || !request.Name.Equals(existingData.Name, StringComparison.CurrentCultureIgnoreCase))
                && await repository.IsNameExistsAsync(request.Name))//if different then check other shouldnot exists
                {
                return new BaseResult<TownCardTypeDto>()
                    {
                    Success = false,
                    Errors = [new(ErrorCode.DuplicateData, translator.GetString($"Name({request.Name}) Already exists"), nameof(request.Name))]
                    };
                }
            return null;
            }
        }
    }