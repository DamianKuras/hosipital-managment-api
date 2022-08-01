using AutoMapper;
using hosipital_managment_api.Data;
using hosipital_managment_api.Dto;

namespace hosipital_managment_api.AutoMapper
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<RegisterDto, ApiUser > ();
            
        }
    }
}
