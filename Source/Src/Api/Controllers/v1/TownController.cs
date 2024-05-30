using MyTown.SharedModels.DTOs;
using MyTown.SharedModels.Features.Towns.Commands;
using MyTown.SharedModels.Features.Towns.Queries;
using PublicCommon;
using SharedResponse;

namespace CleanArchitecture.WebApi.Controllers.v1
    {
    [ApiVersion("1")]
    //format is [("api/v{version:apiVersion}/[controller]/[action]")]
    public class TownController : BaseApiController
        {
        //TODO had to make role wise allowed
        //This Update only by SuperAdmins not anyone else

        [HttpGet]
        public async Task<IReadOnlyList<TownDto>> GetAll()
            {
            Console.WriteLine($"{nameof(TownController)}/{nameof(ApiEndPoints.GetAll)}");
            try
                {
                var res = await Mediator.Send(new GetTownsAllQuery());
                return res;
                }
            catch (Exception e)
                {
                Console.WriteLine(e.ToString());
                throw;
                }
            }

        [HttpGet]
        public async Task<PagedResponse<TownDto>> GetPagedList([FromQuery] GetTownsPagedListQuery model)
            {
            Console.WriteLine($"{nameof(TownController)}/{nameof(ApiEndPoints.GetPagedList)}");
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
        public async Task<BaseResult<TownDto>> GetById([FromQuery] GetTownByIdQuery model)
            {
            Console.WriteLine($"{nameof(TownController)}/{nameof(ApiEndPoints.GetById)}");
            return await Mediator.Send(model);
            }


        [HttpPost, Authorize(Roles = CONSTANTS.Auth.Role_Admin)]
        public async Task<BaseResult<TownDto>> Create(CreateUpdateTownCommand model)
            {
            Console.WriteLine($"{nameof(TownController)}/{nameof(ApiEndPoints.Create)}");
            //model.CreatedBy = UserIdExtract();
            //above is separately not required bcz ApplicationDBcontext amking default changes onCreate Authuser id & onUpdate LastModifedBy userid
            return await Mediator.Send(model);
            }


        [HttpPut, Authorize(Roles = CONSTANTS.Auth.Role_Admin)]
        public async Task<BaseResult<TownDto>> Update(CreateUpdateTownCommand model)
            {
            Console.WriteLine($"{nameof(TownController)}/{nameof(ApiEndPoints.Update)}");
            //model.LastModifiedBy = UserIdExtract();
            return await Mediator.Send(model);
            }

        [HttpDelete, Authorize(Roles = CONSTANTS.Auth.Role_Admin)]
        public async Task<BaseResult> Delete([FromQuery] DeleteTownCommand model)
            {
            Console.WriteLine($"{nameof(TownController)}/{nameof(ApiEndPoints.Delete)}");
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