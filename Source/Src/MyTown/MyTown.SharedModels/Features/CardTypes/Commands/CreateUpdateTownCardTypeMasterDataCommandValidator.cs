using FluentValidation;

namespace MyTown.SharedModels.Features.CardTypes.Commands
    {
    public class CreateUpdateTownCardTypeMasterDataCommandValidator : AbstractValidator<CreateUpdateTownCardTypeMasterDataCommand>
        {
        public CreateUpdateTownCardTypeMasterDataCommandValidator()//(ITranslator translator)
            {
            RuleFor(p => p.Name)
                .NotNull();
            //.WithName(p => translator[nameof(p.Name)]);
            RuleFor(p => p.ShortName)
                .NotNull();
            //.WithName(p => translator[nameof(p.ShortName)]);
            }
        }
    }