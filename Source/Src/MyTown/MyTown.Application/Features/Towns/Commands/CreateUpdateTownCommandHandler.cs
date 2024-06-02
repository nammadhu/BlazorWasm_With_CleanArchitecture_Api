using MyTown.Application.Interfaces;
using MyTown.SharedModels.Features.Towns.Commands;
using SharedResponse;

namespace MyTown.Application.Features.Towns.Commands
    {
    public class CreateUpdateTownCommandHandler(ITownRepository repository, IUnitOfWork unitOfWork,
        ITranslator translator, IMapper mapper, IIDGenerator<Town> idNextGenerator) : IRequestHandler<CreateUpdateTownCommand, BaseResult<TownDto>>
        {
        public async Task<BaseResult<TownDto>> Handle(CreateUpdateTownCommand request, CancellationToken cancellationToken)
            {
            if (request.Id > 0)//UPDATE
                {
                var existingData = await repository.GetByIdIntAsync(request.Id);
                if (existingData is null)
                    {
                    return new BaseResult<TownDto>()
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

                return new BaseResult<TownDto>() { Success = await unitOfWork.SaveChangesAsync(), Data = mapper.Map<TownDto>(existingData) };
                }
            else//create
                {
                var isDuplicateResult = await NameExists(repository, translator, request, null);
                if (isDuplicateResult != null) return isDuplicateResult;

                var obj = request.To<CreateUpdateTownCommand, Town>();
                //todo should modify above 
                //var product = new Town(request.Name, request.Price, request.BarCode);

                obj.Id = idNextGenerator.GetNextID();
                var result = await repository.AddAsync(obj);
                //var success = await unitOfWork.SaveChangesAsync();
                return new BaseResult<TownDto>() { Success = await unitOfWork.SaveChangesAsync(), Data = mapper.Map<TownDto>(result) };
                }
            }

        private static async Task<BaseResult<TownDto>?> NameExists(ITownRepository repository, ITranslator translator, CreateUpdateTownCommand request, Town? existingData)
            {
            if (existingData is null && request.Id > 0)//exisitng shuld have matching record
                {
                return new BaseResult<TownDto>()
                    {
                    Success = false,
                    Errors = [new(ErrorCode.ModelIsNull, translator.GetString($"Data Mapping Is Null"), nameof(request.Id))]
                    };
                }
            if ((existingData == null || !request.Name.Equals(existingData.Name, StringComparison.CurrentCultureIgnoreCase))
                && await repository.IsNameExistsAsync(request.Name))//if different then check other shouldnot exists
                {
                return new BaseResult<TownDto>()
                    {
                    Success = false,
                    Errors = [new(ErrorCode.DuplicateData, translator.GetString($"Name({request.Name}) Already exists"), nameof(request.Name))]
                    };
                }
            return null;
            }
        }
    }