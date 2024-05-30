using FluentValidation;
using SharedResponse;

namespace MyTown.SharedModels.Features.Cards.Commands
    {
    public class UpdateTownCardCommandValidator : AbstractValidator<UpdateTownCardCommand>
        {
        public UpdateTownCardCommandValidator(ITranslator translator)
            {
            //    RuleFor(p => p.MyProperty)
            //        .NotNull()
            //        .WithName(p => translator[nameof(p.MyProperty)]);
            }
        }
    }