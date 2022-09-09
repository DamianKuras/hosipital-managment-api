using AutoMapper;
using hosipital_managment_api.Data;
using hosipital_managment_api.Dto;
using hosipital_managment_api.Models;

namespace hosipital_managment_api.AutoMapper
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<RegisterDto, ApiUser > ();
            CreateMap<Prescription, PrescriptionDisplayDTO>();
            CreateMap<PrescriptionMedicine, PrescriptionMedicineDisplayDTO>();
            CreateMap<Prescription, PrescriptionsListDisplayDTO>();
        }
    }
}
