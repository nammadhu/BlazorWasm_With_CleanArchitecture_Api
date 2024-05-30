using FluentValidation;

namespace MyTown.SharedModels.Features.Towns.Commands
    {
    public class CreateUpdateTownCommandValidator : AbstractValidator<CreateUpdateTownCommand>
        {
        public CreateUpdateTownCommandValidator()//(ITranslator translator)
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