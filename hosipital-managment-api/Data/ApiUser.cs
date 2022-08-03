using Microsoft.AspNetCore.Identity;

namespace hosipital_managment_api.Data
{
   
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }
    }
}