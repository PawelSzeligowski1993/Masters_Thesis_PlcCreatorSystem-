using Newtonsoft.Json.Linq;
using PlcCreatorSystem_Utility;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Services.IServices;

namespace PlcCreatorSystem_WEB.Services
{
    public class PlcService : BaseService, IPlcService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string? plctUrl;
        public PlcService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            plctUrl = configuration.GetValue<string>("ServiceUrls:Creator_API");
        }

        public Task<T> CreateAsync<T>(PlcCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = plctUrl + "/api/PLC_API/",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = plctUrl + "/api/PLC_API/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = plctUrl + "/api/PLC_API/",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = plctUrl + "/api/PLC_API/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(PlcUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = plctUrl + "/api/PLC_API/" + dto.Id,
                Token = token
            });
        }
    }
}
