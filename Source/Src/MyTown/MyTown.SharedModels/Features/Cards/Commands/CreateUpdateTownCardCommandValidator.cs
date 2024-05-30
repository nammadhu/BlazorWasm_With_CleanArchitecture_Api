using FluentValidation;

namespace MyTown.SharedModels.Features.Cards.Commands
    {
    public class CreateUpdateTownCardCommandValidator : AbstractValidator<CreateUpdateTownCardCommand>
        {
        public CreateUpdateTownCardCommandValidator()//(ITranslator translator)
            {
            RuleFor(p => p.Name)
                .NotNull();
            //.WithName(p => translator[nameof(p.Name)]);
            //RuleFor(p => p.ShortName)
            //    .NotNull();
            //.WithName(p => translator[nameof(p.ShortName)]);
            }
        }
    }