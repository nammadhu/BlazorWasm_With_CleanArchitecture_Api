namespace MyTown.Domain
    {

    //db entity
    public class Town : TownBase
        {
        public Town()
            {

            }
        public Town(string title, string subTitle) : base(title, subTitle)
            {

            }

        //move & fetch this from separate tables
        public string District { get; set; } = "Shimoga";//later move to other table called districts & refer here only id 
        public string State { get; set; } = "Karnataka";//later move to other table called states & refer here only id 

        public string? UrlName1 { get; set; }//bhadravathi.com
        public string? UrlName2 { get; set; }//bdvt.in

        //public string? TownImageUrl1 { get; set; }
        //public string? TownImageUrl2 { get; set; }
        //public string? TownImageUrl3 { get; set; }
        //public string? TownImageUrl4 { get; set; }
        //public string? TownImageUrl5 { get; set; }
        //public string? TownYoutubeVideo { get; set; }

        // Navigation property for TownCards
        public virtual ICollection<TownCard> TownCards { get; set; } = new List<TownCard>();
        //virtual - lazy loading, if still needed then use .Inculde(x=>x.TownCards)
        }


    }
