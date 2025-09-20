using PlcCreatorSystem_Utility;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Services.IServices;

namespace PlcCreatorSystem_WEB.Services
{
    public class PLCService : BaseService, IPlcService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string plctUrl;
        public PLCService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            plctUrl = configuration.GetValue<string>("ServiceUrls:Creator_API");
        }

        public Task<T> CreateAsync<T>(PlcCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = plctUrl + "/api/PLC_API/"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = plctUrl + "/api/PLC_API/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = plctUrl + "/api/PLC_API/"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = plctUrl + "/api/PLC_API/" + id
            });
        }

        public Task<T> UpdateAsync<T>(PlcUpdateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = plctUrl + "/api/PLC_API/" + dto.Id
            });
        }
    }
}
