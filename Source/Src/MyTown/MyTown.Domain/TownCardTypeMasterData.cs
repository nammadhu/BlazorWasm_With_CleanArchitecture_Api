using PublicCommon.Common;


namespace MyTown.Domain
    {
    //town table
    //townitemtype master data table
    //townid+ townitem table



    //fetch city related all entities at once & store on client side
    //display city title.subtitle,decription
    //priority & posible solution
    //events
    //business
    //doctor
    //school
    //college
    //real estate
    //buyorsale
    //openissue
    //report complaint
    //jobs vacancies & registrqation of job looking candidates in town
    //feedback or contact
    //no db entity


    //db entity
    public class TownCardTypeMasterData : BaseAuditableEntityMultiUser, IMasterData
        {//this is only masterdata
        public TownCardTypeMasterData()
            {

            }
        public TownCardTypeMasterData(string name)
            {
            Name = name;
            ShortName = name;
            }
        public TownCardTypeMasterData(string name, string shortName)
            {
            Name = name;
            ShortName = shortName;
            }

        //[Key]
        //public int Id { get; set; }
        //this should not  be appeared to front end screen to users
        public byte ApplicationTypeId { get; set; } = 1;//1 for townitem,2 for holige products
        public string Name { get; set; }//1:Town,2doctor,business,event,advertisement
        public string ShortName { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; } = 1;//100
        public byte? PriorityOrder { get; set; } = 1;
        //todo later will make it up in each card by user charging onrequest moving up or first kind of

        //not using this,instead using automapper
        //public void Update(string name, string shortName, string description, int price)
        //    {
        //    Name = name;
        //    ShortName = shortName;
        //    Description = description;
        //    Price = price;
        //    }
        /*
        public static TownCardTypeMasterData? Get(int id)
            {
            return StandardList.Find(x => x.Id == id);
            }
        public static TownCardTypeMasterData? GetFirst(string nameContains)
            {
            return StandardList.Where(x => x.Name.Contains(nameContains, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            }
        public static List<TownCardTypeMasterData>? GetList(string nameContains)
            {
            return StandardList.Where(x => x.Name.Contains(nameContains, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
        public static readonly List<TownCardTypeMasterData> StandardList = new List<TownCardTypeMasterData>(){
            new TownCardTypeMasterData(0,"Town"),
            new TownCardTypeMasterData(1,"Priority Message"),//flash message
            new TownCardTypeMasterData(2,"Event"),
                 new TownCardTypeMasterData(3,"Premium Shops"),
            new TownCardTypeMasterData(4,"Doctor Clinic Hospital"),
            new TownCardTypeMasterData(5,"School College Tuition"),

            //business types
            new TownCardTypeMasterData(11,"Vehicle Garage Bike Car Scooter","Vehicle Garage"),
            new TownCardTypeMasterData(11,"Hotel Lodge Restaurant"),
            new TownCardTypeMasterData(11,"Textiles Tailors Designers"),
            new TownCardTypeMasterData(11,"Beauticians Saloons Hair Cut"),
            new TownCardTypeMasterData(11,"Electricals Home Appliances"),
            new TownCardTypeMasterData(11,"Choultry & Convention Hall"),
            new TownCardTypeMasterData(11,"Shops,Provision Stores,Super Markets"),//Jewellary,saw mills
            new TownCardTypeMasterData(11,"Gas Agency Petrol Bunks"),
            new TownCardTypeMasterData(11,"Bank,Govt Offices"),


             new TownCardTypeMasterData(7,"Real Estate"),
            new TownCardTypeMasterData(8,"Buy Or sale"),
            new TownCardTypeMasterData(9,"Open Issue"),
            new TownCardTypeMasterData(10,"Jobs Available"),
            new TownCardTypeMasterData(11,"Add Resume"),
            //user complaints
            };
        */
        }

    }
