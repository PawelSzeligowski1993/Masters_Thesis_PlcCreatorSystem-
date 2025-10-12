using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PlcCreatorSystem_Utility;
using PlcCreatorSystem_WEB.Models;
using PlcCreatorSystem_WEB.Models.Dto;
using PlcCreatorSystem_WEB.Services.IServices;
using System.Net.Http.Headers;

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

        public async Task<APIResponse> UploadCsvAsync(int projectId, IFormFile csvFile, string token)
        {
            if (csvFile == null || csvFile.Length == 0)
                return new APIResponse { IsSuccess = false, ErrorsMessages = new List<string> { "CSV file is required." } };

            var client = httpClient.CreateClient("PLC_CREATOR_SYSTEM_API");

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            using var form = new MultipartFormDataContent();
            using var fileContent = new StreamContent(csvFile.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            form.Add(fileContent, "csvFile", csvFile.FileName);

            var resp = await client.PostAsync($"/api/Project_API/{projectId}/csv", form);

            var body = await resp.Content.ReadAsStringAsync();
            var api = JsonConvert.DeserializeObject<APIResponse>(body) ?? new APIResponse();

            if (!resp.IsSuccessStatusCode)
                api.IsSuccess = false;

            return api;
        }
    }
}
