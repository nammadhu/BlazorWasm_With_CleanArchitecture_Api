using CleanArchitecture.Application.Interfaces.UserInterfaces;
using CleanArchitecture.Domain;
using Microsoft.AspNetCore.Identity;
using MyTown.SharedModels.DTOs;
using MyTown.SharedModels.Features.Cards.Commands;
using MyTown.SharedModels.Features.Cards.Queries;
using PublicCommon;
using SharedResponse;
using System.Collections.Generic;

namespace CleanArchitecture.WebApi.Controllers.v1
    {
    [ApiVersion("1")]
    //format is [("api/v{version:apiVersion}/[controller]/[action]")]
    public class TownCardController(IAccountServices accountServices) : BaseApiController
        {
        //TODO had to make role wise allowed
        //This Update only by SuperAdmins not anyone else


        void GetMy(bool isCreator,bool isOwner,bool isReviewer)
            {




            //Tuple(List{ approvedId},List{ draftcards}, List{ approvalsWaiting@})
            }

        //[HttpGet]//dont call this
        private async Task<IReadOnlyList<TownCardDto>> GetAll()
            {
            Console.WriteLine($"{nameof(TownCardController)}/{nameof(ApiEndPoints.GetAll)}");
            try
                {
                var res = await Mediator.Send(new GetTownCardsAllQuery());
                return res;
                }
            catch (Exception e)
                {
                Console.WriteLine(e.ToString());
                throw;
                }
            }

        [HttpGet]
        public async Task<PagedResponse<TownCardDto>> GetPagedList([FromQuery] GetTownCardsPagedListQuery model)
            {
            Console.WriteLine($"{nameof(TownCardController)}/{nameof(ApiEndPoints.GetPagedList)}");
            try
                {
                var res = await Mediator.Send(model);
                return res;
                }
            catch (Exception e)
                {
                Console.WriteLine(e.ToString());
                throw;
                }
            }

        [HttpGet]
        public async Task<BaseResult<TownCardDto>> GetById([FromQuery] GetTownCardByIdQuery model)
            {
            Console.WriteLine($"{nameof(TownCardController)}/{nameof(ApiEndPoints.GetById)}");
            return await Mediator.Send(model);
            }


        [HttpPost, Authorize]//lets allow everyone to create,later will block Blocked users
        public async Task<BaseResult<TownCardDto>> Create(CreateUpdateTownCardCommand model)
            {
            //once user created had to mark him with role of Creator
            Console.WriteLine($"{nameof(TownCardController)}/{nameof(ApiEndPoints.Create)}");
            //model.CreatedBy = UserIdExtract();
            //above is separately not required bcz ApplicationDBcontext amking default changes onCreate Authuser id & onUpdate LastModifedBy userid
            var result = await Mediator.Send(model);
            if (result.Success)
                {
                //add role of CardCreator
                await accountServices.AddCurrentUserToRole(CONSTANTS.Auth.Role_CardCreator);
                //todo CreatedCardCount+1... target also ApprovedCardCount not here
                }
            return result;
            }


        [HttpPut, Authorize]//lets allow everyone to create,later will block Blocked users
        public async Task<BaseResult<TownCardDto>> Update(CreateUpdateTownCardCommand model)
            {
            Console.WriteLine($"{nameof(TownCardController)}/{nameof(ApiEndPoints.Update)}");
            //model.LastModifiedBy = UserIdExtract();
            return await Mediator.Send(model);
            }

        [HttpDelete, Authorize]//lets allow everyone to create,later will block Blocked users
        public async Task<BaseResult> Delete([FromQuery] DeleteTownCardCommand model)
            {
            Console.WriteLine($"{nameof(TownCardController)}/{nameof(ApiEndPoints.Delete)}");
            //model.LastModifiedBy = UserIdExtract();
            return await Mediator.Send(model);
            }

        //private Guid UserIdExtract()
        //    {
        //    //this is separately not required bcz ApplicationDBcontext amking default changes onCreate Authuser id & onUpdate LastModifedBy userid
        //    var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (Guid.TryParse(id, out Guid guid))
        //        {
        //        return guid;
        //        }
        //    throw new Exception("UserId Not Found");
        //    }
        }
    }