using PublicCommon.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTown.Domain
    {

    public class TownApprovedCard : TownBase
        {
        //no direct operation here by creator,instead all goes to card
        public TownApprovedCard()
            {
            SelectedDates = [];//new HashSet<TownApprovedCardSelectedDate>();
            OwnerId = CreatedBy;  //LastModifiedBy ?? CreatedBy;
            }
        public void AddDate(TownApprovedCardSelectedDate date)
            {
            // The HashSet will handle uniqueness and performance efficiently
            SelectedDates.Add(date);
            }
        [Required]
        public int CardId { get; set; }//doctor,event,business

        [ForeignKey(nameof(CardId))]
        public TownCard? Card { get; set; }



        [Required]
        public int TypeId { get; set; }//doctor,event,business

        [ForeignKey(nameof(TypeId))]
        public TownCardType? Type { get; set; }

       

        //ideally this has to be set on transfer ownership kind of but //todo
        public Guid OwnerId { get; set; }
        public virtual ICollection<TownApprovedCardSelectedDate> SelectedDates { get; set; }

        [Required]
        public int TownId { get; set; } //bhadravathi,kadur,bidar


        //separatetable "TownCardApproval"
        public int ApprovedCount { get; set; } //by equal or above grade people
        public int RejectedCount { get; set; } //by equal or above grade people

        //TownItemLikeDisLike
        public int LikeCount { get; set; }//by public anyone
        public int DisLikeCount { get; set; }//by public anyone
        }
    public class TownApprovedCardSelectedDate : BaseAuditableEntityMultiUser
        {
        //public int Id { get; set; }
        public int ApprovedCardId { get; set; }
        public DateOnly Date { get; set; }

        // Navigation property to the ApprovedCard
        [ForeignKey(nameof(ApprovedCardId))]
        public TownApprovedCard? ApprovedCard { get; set; }
        }

    //each called iCard , internet card of any user or business entity
    //dbentity, once approved then will be moving the approved entity to ApprovedCard table
    public class TownCard : TownBase
        {
        public TownCard()
            {
            OwnerId = CreatedBy;  //LastModifiedBy ?? CreatedBy;
            //this is must for EF cores migration & all
            }
        public TownCard(TownCardType type, string title):this()
            {
            Type = type;
            Name = title;
            }
        public TownCard(TownCardType type, string title, string subtitle) : this(type, title)
            {
            SubTitle = subtitle;
            }
        public TownCard(TownCardType type, int id, string title, string subtitle) : this(type, title, subtitle)
            {
            //this should be removed later,as id is from db or from screen its 0/null only
            Id = id;
            }

        //ideally this has to be set on transfer ownership kind of but //todo
        public Guid OwnerId { get; set; }

        public int? ApprovedCardId { get; set; }//if approved then that will be linked here

        //[ForeignKey(nameof(ApprovedCardId))]
        //public TownApprovedCard ApprovedCard { get; set; }
        [Required]
        public int TypeId { get; set; }//doctor,event,business

        [ForeignKey(nameof(TypeId))]
        public TownCardType? Type { get; set; }


        [Required]
        public int TownId { get; set; } //bhadravathi,kadur,bidar

        //[ForeignKey(nameof(TownId))]
        //public Town Town { get; set; }

        }

    public class TownCardApproval : BaseAuditableEntitySingleUser
        {

        [Required]
        public int TownCardId { get; set; } //bhadravathi,kadur,bidar

        [ForeignKey(nameof(TownCardId))]
        public TownCard TownCard { get; set; }

        //when creator requested Approved is null then either he can approve or reject
        public bool? Approved { get; set; }

        [Display(Name = "ApproverId")]
        public override Guid UserId { get; set; }


        public string? Message { get; set; }
        }


    //later
    public class TownCardLikes : BaseAuditableEntitySingleUser
        {

        [Required]
        public int TownCardId { get; set; } //bhadravathi,kadur,bidar

        [ForeignKey(nameof(TownCardId))]
        public TownCard TownCard { get; set; }



        [Display(Name = "ByUserId")]
        public override Guid UserId { get; set; }


        public bool Liked { get; set; }
        //either like or rating anyone might be enough //later
        public int Rating { get; set; }
        public string? Message { get; set; }
        }

    //later
    public class TownCardTag : BaseAuditableEntityMultiUser
        {
        [Required]
        public int TownCardId { get; set; } //bhadravathi,kadur,bidar

        [ForeignKey(nameof(TownCardId))]
        public TownCard TownCard { get; set; }

        public required string Tag { get; set; }
        }

    }
