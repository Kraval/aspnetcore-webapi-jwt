using System;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreWebApiJwt.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Url {get;set;}
    }
}