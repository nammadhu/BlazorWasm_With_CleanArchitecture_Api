using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Domain;
    public class ApplicationRole(string name) : IdentityRole<Guid>(name)
        {
        }