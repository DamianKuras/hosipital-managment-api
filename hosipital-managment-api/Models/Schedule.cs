using hosipital_managment_api.Data;

namespace hosipital_managment_api.Models
{
    public class Schedule
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public TimeOnly TimePerPatient { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End {get;set;}
    }
}
