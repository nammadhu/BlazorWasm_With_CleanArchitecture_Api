using FluentValidation;

namespace MyTown.SharedModels.Features.CardTypes.Commands
    {
    public class CreateUpdateTownCardTypeCommandValidator : AbstractValidator<CreateUpdateTownCardTypeCommand>
        {
        public CreateUpdateTownCardTypeCommandValidator()//(ITranslator translator)
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