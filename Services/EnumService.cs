using JobTrackingUI.Helpers;
using Newtonsoft.Json;

namespace JobTrackingUI.Services;

public class EnumService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<EnumItem>> GetActionTypesAsync()
    {
        return await GetEnumList("action-types");
    }

    public async Task<List<EnumItem>> GetApplicationStatusesAsync()
    {
        return await GetEnumList("application-statuses");
    }

    public async Task<List<EnumItem>> GetContractTypesAsync()
    {
        return await GetEnumList("contract-types");
    }

    public async Task<List<EnumItem>> GetCurrenciesAsync()
    {
        return await GetEnumList("currencies");
    }

    public async Task<List<EnumItem>> GetJobSourcesAsync()
    {
        return await GetEnumList("job-sources");
    }

    public async Task<List<EnumItem>> GetPrioritiesAsync()
    {
        return await GetEnumList("priorities");
    }

    public async Task<List<EnumItem>> GetRejectionReasonsAsync()
    {
        return await GetEnumList("rejection-reasons");
    }

    public async Task<AllEnums> GetAllEnumsAsync()
    {
        var response = await _httpClient.GetAsync($"/api/enums/all");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var enumList = JsonConvert.DeserializeObject<AllEnums>(content);
            return enumList ?? new AllEnums();
        }
        else
        {
            throw new Exception("Error fetching all enums list");
        }
    }

    private async Task<List<EnumItem>> GetEnumList(string enumName)
    {
        var response = await _httpClient.GetAsync($"/api/enums/{enumName}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var enumList = JsonConvert.DeserializeObject<List<EnumItem>>(content);
            return enumList ?? new List<EnumItem>();
        }
        else
        {
            throw new Exception("Error fetching enum list");
        }
    }
}
