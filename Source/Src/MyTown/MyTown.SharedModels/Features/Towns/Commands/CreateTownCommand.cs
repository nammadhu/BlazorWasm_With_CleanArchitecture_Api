namespace MyTown.SharedModels.Features.Towns.Commands
    {
    //not using this,instead using CreateUpdate
    public class CreateTownCommand : Town,//later should remove this domain type
        IRequest<BaseResult<int>>
        {
        //public int MyProperty { get; set; }
        }
    }