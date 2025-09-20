using PlcCreatorSystem_Utility;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Services.IServices;

namespace PlcCreatorSystem_WEB.Services
{
    public class HmiService : BaseService, IHmiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string hmiUrl;
        public HmiService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            hmiUrl = configuration.GetValue<string>("ServiceUrls: Creator_API");
        }

        public Task<T> CreateAsync<T>(HmiCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = hmiUrl + "/api/HMI_API/"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = hmiUrl + "/api/HMI_API/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = hmiUrl + "/api/HMI_API/"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = hmiUrl + "/api/HMI_API/" + id
            });
        }

        public Task<T> UpdateAsync<T>(HmiUpdateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = hmiUrl + "/api/HMI_API/" + dto.Id
            });
        }
    }
}
