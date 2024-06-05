using FluentValidation;
using MyTown.SharedModels.Features.Cards.Commands;
using SharedResponse;

namespace MyTown.SharedModels.Features.Cards.Queries
    {
    public class GetUserCardsMoreDetails : IRequest<(List<int> approvedCardIds, List<TownCardDto> draftCards)>
        {
        public int TownId { get; set; }

        public Guid UserId { get; set; }
        //Fetch from IAuthenticationService for logged in user & asssign in controller levels. if null then dont call itslef

        public bool IsCardCreator { get; set; }
        public bool IsCardOwner { get; set; }
        public bool IsCardApprovedOwner { get; set; }
        public bool IsCardApprovedReviewer { get; set; }



        // Guid userId, bool isCreator, bool isOwner, bool isApprovedCardOwner, bool isApprovedReviewer, int townId = 0
        }

    public class GetUserCardMoreDetailsValidator : AbstractValidator<GetUserCardsMoreDetails>
        {
        public GetUserCardMoreDetailsValidator(ITranslator translator)
            {
            //RuleFor(p => p.IsCreator)
            //    .NotNull();
            }
        }
    }
