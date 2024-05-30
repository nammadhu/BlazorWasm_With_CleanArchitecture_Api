namespace MyTown.Domain
    {

    //db entity
    public class Town : TownBase
        {
        public Town()
        {
            
        }
        public Town(string title,string subTitle):base(title,subTitle)
            {

            }

        //move & fetch this from separate tables
        public string District { get; set; } = "Shimoga";//later move to other table called districts & refer here only id 
        public string State { get; set; } = "Karnataka";//later move to other table called states & refer here only id 

        }


    }
