using Microsoft.AspNetCore.Identity;

namespace hosipital_managment_api.Data
{
   
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public DateOnly BirthDay { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Postcode { get; set; }
    }
}