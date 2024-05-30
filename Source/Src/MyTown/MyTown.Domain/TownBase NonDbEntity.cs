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
        public string? ImageUrl { get; set; }
        public string? Address { get; set; }

        public string? MobileNumber { get; set; }
        public string? GoogleMapAddressUrl { get; set; }


        public DateTime? EndDateToShow { get; set; }//after this date content will be removed on screen
        //public byte? PriorityOrder { get; set; }

        //we can make these as like json & store if more links needed
        public string? GoogleProfileUrl { get; set; }
        public string? FaceBookUrl { get; set; }
        public string? YouTubeUrl { get; set; }
        public string? InstagramUrl { get; set; }

        public string? TwitterUrl { get; set; }

        public string? OtherReferenceUrl { get; set; }

        /*
        public void Update(string name, string subTitle, string description, int price)
            {
            Name = name;
            SubTitle = subTitle;
            Description = description;
            }*/

        }


    }
