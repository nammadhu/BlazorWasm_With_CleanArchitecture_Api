using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain
    {
    public class ApplicationUser : IdentityUser<Guid>
        {
        public ApplicationUser()
            {
            Created = DateTime.Now;
            }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        //public DateTime LastModified { get; set; }

        [NotMapped]
        public List<string> Roles { get; set; }
        }
    }
