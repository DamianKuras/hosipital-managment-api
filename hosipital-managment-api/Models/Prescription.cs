using hosipital_managment_api.Data;

namespace hosipital_managment_api.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public ApiUser Patient { get; set; }
        public ApiUser Doctor{get;set;}
        public DateTime Created_at { get; set; }
        public DateOnly ExpDate { get; set; }
    }
}
