using hosipital_managment_api.Data;

namespace hosipital_managment_api.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public ApiUser Patient { get; set; }
        public string PatientId { get; set; }
        public ApiUser Doctor { get; set; }
        public string DoctorId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Status { get; set; }
        
    }
}
