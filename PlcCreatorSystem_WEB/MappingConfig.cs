using AutoMapper;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;

namespace PlcCreatorSystem_WEB
{
    public class MappingConfig :Profile
    {
        public MappingConfig()
        {
            //Project Mapper
            CreateMap<ProjectDTO, ProjectCreateDTO>().ReverseMap();
            CreateMap<ProjectDTO, ProjectUpdateDTO>().ReverseMap();

            //HMI Mapper
            CreateMap<HmiDTO, HmiCreateDTO>().ReverseMap();
            CreateMap<HmiDTO, HmiUpdateDTO>().ReverseMap();

            //PLC Mapper
            CreateMap<PlcDTO, PlcCreateDTO>().ReverseMap();
            CreateMap<PlcDTO, PlcUpdateDTO>().ReverseMap();

            //User Mapper
            CreateMap<UserDTO, UserUpdateDTO>().ReverseMap();
        }
    }
}
