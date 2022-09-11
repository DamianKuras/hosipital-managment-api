using hosipital_managment_api.Data;

namespace hosipital_managment_api.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public TimeOnly TimePerPatient { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End {get;set;}
    }
}
