using JobTrackingUI.PageModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JobTrackingUI.Services;

public class ApplicationService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<PaginatedModel<ApplicationModel>> GetApplicationsAsync()
    {
        var response = await _httpClient.GetAsync("/api/applications");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var applications = JsonConvert.DeserializeObject<PaginatedModel<ApplicationModel>>(content);
        return applications ?? new PaginatedModel<ApplicationModel>();
    }
}