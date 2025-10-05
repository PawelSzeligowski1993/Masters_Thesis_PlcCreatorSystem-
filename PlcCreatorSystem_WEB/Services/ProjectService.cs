using Microsoft.Extensions.Configuration;
using PlcCreatorSystem_Utility;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Services.IServices;

namespace PlcCreatorSystem_WEB.Services
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string projectUrl;
        public ProjectService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            projectUrl = configuration.GetValue<string>("ServiceUrls:Creator_API");
        }

        public Task<T> CreateAsync<T>(ProjectCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = projectUrl + "/api/Project_API/",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = projectUrl + "/api/Project_API/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = projectUrl + "/api/Project_API/",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = projectUrl + "/api/Project_API/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(ProjectUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = projectUrl + "/api/Project_API/" + dto.Id,
                Token = token
            });
        }
    }
}
