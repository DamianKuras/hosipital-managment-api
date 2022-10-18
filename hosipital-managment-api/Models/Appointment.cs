using hosipital_managment_api.Data;
using System.ComponentModel.DataAnnotations;

namespace hosipital_managment_api.Models
{
    public class Appointment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public ApiUser Patient { get; set; }
        public string PatientId { get; set; }
        public ApiUser Doctor { get; set; }
        public string DoctorId { get; set; }

        [Required]
        public DateTimeOffset Date { get; set; }

        [Required]
        public string Status { get; set; }
        
    }
}
