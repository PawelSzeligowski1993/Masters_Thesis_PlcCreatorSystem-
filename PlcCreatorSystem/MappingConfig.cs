using AutoMapper;
using PlcCreatorSystem_API.Models;
using PlcCreatorSystem_API.Models.Dto;

namespace PlcCreatorSystem_API
{
    public class MappingConfig :Profile
    {
        public MappingConfig()
        {
            //Project Mapper
            CreateMap<Project, ProjectDTO>().ReverseMap();
            CreateMap<Project, ProjectCreateDTO>().ReverseMap();
            CreateMap<Project, ProjectUpdateDTO>().ReverseMap();

            //HMI Mapper
            CreateMap<HMI, HmiDTO>().ReverseMap();
            CreateMap<HMI, HmiCreateDTO>().ReverseMap();
            CreateMap<HMI, HmiUpdateDTO>().ReverseMap();

            //PLC Mapper
            CreateMap<PLC, PlcDTO>().ReverseMap();
            CreateMap<PLC, PlcCreateDTO>().ReverseMap();
            CreateMap<PLC, PlcUpdateDTO>().ReverseMap();
        }
    }
}
