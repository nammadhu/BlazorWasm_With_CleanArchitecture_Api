using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Domain
    {
    public class ApplicationRole : IdentityRole<Guid>
        {
        public ApplicationRole(string name) : base(name)
            {
            }
        }
    }