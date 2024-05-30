using PublicCommon.Common;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.MyTown.Entities
    {

    //later not yet included in db schema
    public class Wallet : BaseAuditableEntityMultiUser
        {
        public override int Id { get; set; }

        [Key]
        public override Guid UserId { get; set; } // Foreign key to ApplicationUser

        //[ForeignKey(nameof(UserId))]//its on differnt table so no link here
        //public ApplicationUser User { get; set; }

        public float Balance { get; set; }//decimal is constly

        // Other properties as needed
        }

    }
