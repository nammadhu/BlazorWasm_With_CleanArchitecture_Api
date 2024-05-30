using MyTown.SharedModels.DTOs;
using MyTown.SharedModels.Features.Cards.Commands;
using MyTown.SharedModels.Features.Cards.Queries;
using PublicCommon;
using SharedResponse;

namespace CleanArchitecture.WebApi.Controllers.v1
    {
    [ApiVersion("1")]
    //format is [("api/v{version:apiVersion}/[controller]/[action]")]
    public class TownCardController : BaseApiController
        {
        //TODO had to make role wise allowed
        //This Update only by SuperAdmins not anyone else

        [HttpGet]
        public async Task<IReadOnlyList<TownCardDto>> GetAll()
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


        [HttpPost, Authorize(Roles = CONSTANTS.Auth.Role_Admin)]
        public async Task<BaseResult<TownCardDto>> Create(CreateUpdateTownCardCommand model)
            {
            Console.WriteLine($"{nameof(TownCardController)}/{nameof(ApiEndPoints.Create)}");
            //model.CreatedBy = UserIdExtract();
            //above is separately not required bcz ApplicationDBcontext amking default changes onCreate Authuser id & onUpdate LastModifedBy userid
            return await Mediator.Send(model);
            }


        [HttpPut, Authorize(Roles = CONSTANTS.Auth.Role_Admin)]
        public async Task<BaseResult<TownCardDto>> Update(CreateUpdateTownCardCommand model)
            {
            Console.WriteLine($"{nameof(TownCardController)}/{nameof(ApiEndPoints.Update)}");
            //model.LastModifiedBy = UserIdExtract();
            return await Mediator.Send(model);
            }

        [HttpDelete, Authorize(Roles = CONSTANTS.Auth.Role_Admin)]
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