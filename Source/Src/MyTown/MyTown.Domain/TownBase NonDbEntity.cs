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

    //TownBase NonDbEntity
    public class TownBase : BaseAuditableEntityMultiUser
        {
        public TownBase()
            {
            //this should never be called,1ly for the sake of EF cores
            }
        public TownBase(string title)
            {
            Name = title;
            }

        public TownBase(string title, string subtitle) : this(title)
            {
            SubTitle = subtitle;
            }

        public TownBase(int id, string title, string subtitle) : this(title, subtitle)
            {
            //this should be removed later,as id is from db or from screen its 0/null only
            Id = id;
            }

        public bool Active { get; set; }
        public string Name { get; set; }
        public string? SubTitle { get; set; }//qualification,type of business,home/hotel/veg/nonveg
        public string? Description { get; set; }
      
        }


    }
