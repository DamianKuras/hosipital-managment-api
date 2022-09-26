using hosipital_managment_api.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hosipital_managment_api.Models
{
    public class Prescription
    {
        public int Id { get; set; }

        public ApiUser Patient { get; set; }

        public string PatientId { get; set; }

        public ApiUser Doctor {get;set;}

        public string DoctorId { get; set; }

        [Required]
        public DateTimeOffset Created_at { get; set; }

        [Required]
        public DateOnly ExpDate { get; set; }
    }
}
