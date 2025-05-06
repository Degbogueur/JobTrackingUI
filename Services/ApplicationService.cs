using System.Net.Http.Headers;
using JobTrackingUI.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JobTrackingUI.Services;

public class ApplicationService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<PaginatedModel<ApplicationModel>> GetApplicationsAsync(
        int pageIndex = 1,
        int pageSize = 9)
    {
        //var response = await _httpClient.GetAsync("/api/applications");
        var response = await _httpClient.GetAsync($"/api/applications?pageIndex={pageIndex}&pageSize={pageSize}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var applications = JsonConvert.DeserializeObject<PaginatedModel<ApplicationModel>>(content);
        return applications ?? new PaginatedModel<ApplicationModel>();
    }

    public async Task<ApplicationModel> GetApplicationByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/applications/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var application = JsonConvert.DeserializeObject<ApplicationModel>(content);
        return application ?? new ApplicationModel();
    }

    public async Task<bool> CreateApplicationAsync(CreateApplicationModel application)
    {
        using var form = new MultipartFormDataContent();

        if (application.Resume != null)
        {
            var resumeContent = new StreamContent(application.Resume.OpenReadStream());
            resumeContent.Headers.ContentType = new MediaTypeHeaderValue(application.Resume.ContentType);
            form.Add(resumeContent, "Resume", application.Resume.FileName);
        }

        if (application.CoverLetter != null)
        {
            var coverLetterContent = new StreamContent(application.CoverLetter.OpenReadStream());
            coverLetterContent.Headers.ContentType = new MediaTypeHeaderValue(application.CoverLetter.ContentType);
            form.Add(coverLetterContent, "CoverLetter", application.CoverLetter.FileName);
        }

        form.Add(new StringContent(application.ApplicationDate.ToString("o")), "ApplicationDate");
        form.Add(new StringContent(application.JobTitle), "JobTitle");
        form.Add(new StringContent(application.JobDescription ?? string.Empty), "JobDescription");
        form.Add(new StringContent(application.CompanyName), "CompanyName");
        form.Add(new StringContent(application.Location), "Location");
        form.Add(new StringContent(application.Source.ToString()), "Source");
        form.Add(new StringContent(application.ContractType.ToString()), "ContractType");
        form.Add(new StringContent(application.OfferUrl), "OfferUrl");
        form.Add(new StringContent(application.PostingDate?.ToString("o") ?? string.Empty), "PostingDate");
        form.Add(new StringContent(application.ClosingDate?.ToString("o") ?? string.Empty), "ClosingDate");
        form.Add(new StringContent(application.Status.ToString()), "Status");
        form.Add(new StringContent(application.LastAction.ToString()), "LastAction");
        form.Add(new StringContent(application.LastActionDate.ToString("o")), "LastActionDate");
        form.Add(new StringContent(application.NextAction.ToString()), "NextAction");
        form.Add(new StringContent(application.NextActionDate?.ToString("o") ?? string.Empty), "NextActionDate");
        form.Add(new StringContent(application.Priority?.ToString() ?? string.Empty), "Priority");
        form.Add(new StringContent(application.Notes ?? string.Empty), "Notes");
        form.Add(new StringContent(application.MinSalaryProposed?.ToString() ?? string.Empty), "MinSalaryProposed");
        form.Add(new StringContent(application.MaxSalaryProposed?.ToString() ?? string.Empty), "MaxSalaryProposed");
        form.Add(new StringContent(application.MinSalaryOffered?.ToString() ?? string.Empty), "MinSalaryOffered");
        form.Add(new StringContent(application.MaxSalaryOffered?.ToString() ?? string.Empty), "MaxSalaryOffered");
        form.Add(new StringContent(application.Currency.ToString() ?? string.Empty), "Currency");
        form.Add(new StringContent(application.KeyWords ?? string.Empty), "KeyWords");
        form.Add(new StringContent(application.InterestLevel.ToString()), "InterestLevel");
        form.Add(new StringContent(application.ContactName ?? string.Empty), "ContactName");
        form.Add(new StringContent(application.ContactEmail ?? string.Empty), "ContactEmail");

        var response = await _httpClient.PostAsync("/api/applications", form);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            var errorMessage = await GetErrorMessageAsync(response);
            throw new Exception($"Error creating application: {errorMessage}");
        }
    }

    public async Task<bool> UpdateApplicationAsync(int id, EditApplicationModel application)
    {
        using var form = new MultipartFormDataContent();

        if (application.Resume != null)
        {
            var resumeContent = new StreamContent(application.Resume.OpenReadStream());
            resumeContent.Headers.ContentType = new MediaTypeHeaderValue(application.Resume.ContentType);
            form.Add(resumeContent, "Resume", application.Resume.FileName);
        }

        if (application.CoverLetter != null)
        {
            var coverLetterContent = new StreamContent(application.CoverLetter.OpenReadStream());
            coverLetterContent.Headers.ContentType = new MediaTypeHeaderValue(application.CoverLetter.ContentType);
            form.Add(coverLetterContent, "CoverLetter", application.CoverLetter.FileName);
        }
        form.Add(new StringContent(application.ApplicationDate.ToString("o")), "ApplicationDate");
        form.Add(new StringContent(application.JobTitle), "JobTitle");
        form.Add(new StringContent(application.JobDescription ?? string.Empty), "JobDescription");
        form.Add(new StringContent(application.CompanyName), "CompanyName");
        form.Add(new StringContent(application.Location), "Location");
        form.Add(new StringContent(application.Source.ToString()), "Source");
        form.Add(new StringContent(application.ContractType.ToString()), "ContractType");
        form.Add(new StringContent(application.OfferUrl), "OfferUrl");
        form.Add(new StringContent(application.PostingDate?.ToString("o") ?? string.Empty), "PostingDate");
        form.Add(new StringContent(application.ClosingDate?.ToString("o") ?? string.Empty), "ClosingDate");
        form.Add(new StringContent(application.Status.ToString()), "Status");
        form.Add(new StringContent(application.LastAction.ToString()), "LastAction");
        form.Add(new StringContent(application.LastActionDate.ToString("o")), "LastActionDate");
        form.Add(new StringContent(application.NextAction.ToString()), "NextAction");
        form.Add(new StringContent(application.NextActionDate?.ToString("o") ?? string.Empty), "NextActionDate");
        form.Add(new StringContent(application.Priority?.ToString() ?? string.Empty), "Priority");
        form.Add(new StringContent(application.Notes ?? string.Empty), "Notes");
        form.Add(new StringContent(application.MinSalaryProposed?.ToString() ?? string.Empty), "MinSalaryProposed");
        form.Add(new StringContent(application.MaxSalaryProposed?.ToString() ?? string.Empty), "MaxSalaryProposed");
        form.Add(new StringContent(application.MinSalaryOffered?.ToString() ?? string.Empty), "MinSalaryOffered");
        form.Add(new StringContent(application.MaxSalaryOffered?.ToString() ?? string.Empty), "MaxSalaryOffered");
        form.Add(new StringContent(application.Currency.ToString() ?? string.Empty), "Currency");
        form.Add(new StringContent(application.KeyWords ?? string.Empty), "KeyWords");
        form.Add(new StringContent(application.InterestLevel.ToString()), "InterestLevel");
        form.Add(new StringContent(application.ContactName ?? string.Empty), "ContactName");
        form.Add(new StringContent(application.ContactEmail ?? string.Empty), "ContactEmail");
        form.Add(new StringContent(application.RejectionReason.ToString() ?? string.Empty), "RejectionReason");

        var response = await _httpClient.PutAsync($"/api/applications/{id}", form);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            var errorMessage = await GetErrorMessageAsync(response);
            throw new Exception($"Error updating application: {errorMessage}");
        }
    }

    public async Task DeleteApplicationAsync(int id)
    {
        var response = await _httpClient.PatchAsync($"/api/applications/{id}/delete", null);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageAsync(response);
            throw new Exception($"Error deleting application: {errorMessage}");
        }
    }

    private static async Task<string> GetErrorMessageAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        try
        {
            var errorObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            if (errorObj?.TryGetValue("error", out var message) == true)
                return message;

            var problem = JsonConvert.DeserializeObject<ProblemDetails>(content);
            return problem?.Detail ?? "Une erreur est survenue.";
        }
        catch
        {
            return "Une erreur inattendue s'est produite.";
        }
    }

}