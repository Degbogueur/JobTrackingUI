using System.Net.Http.Headers;
using JobTrackingUI.Helpers;
using JobTrackingUI.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace JobTrackingUI.Services;

public class ApplicationService(HttpClient httpClient, FileHelpers fileHelpers)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly FileHelpers _fileHelpers = fileHelpers;

    public async Task<PaginatedModel<ApplicationModel>> GetApplicationsAsync(
        QueryParameters? parameters = null)
    {
        var queryParameters = GetQueryParameters(parameters);

        var url = QueryHelpers.AddQueryString("/api/applications", queryParameters!);
        var response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var applications = JsonConvert.DeserializeObject<PaginatedModel<ApplicationModel>>(content);
        return applications ?? new PaginatedModel<ApplicationModel>();
    }

    public async Task<PaginatedModel<ApplicationModel>> GetDeletedApplicationsAsync(
        QueryParameters? parameters = null)
    {
        var queryParameters = GetQueryParameters(parameters);

        var url = QueryHelpers.AddQueryString("/api/applications/trash", queryParameters!);
        var response = await _httpClient.GetAsync(url);

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
        if (application != null)
        {
            application.ResumeFilePath = _fileHelpers.GetFilePath(application.ResumeFilePath) ?? string.Empty;
            application.CoverLetterFilePath = _fileHelpers.GetFilePath(application.CoverLetterFilePath);
                return application;
        }
        return new ApplicationModel();
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

    public async Task UpdateApplicationStatusAsync(int id, UpdateApplicationStatusModel model)
    {
        var json = JsonConvert.SerializeObject(model);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PatchAsync($"/api/applications/{id}/update-status", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageAsync(response);
            throw new Exception($"Error updating application status: {errorMessage}");
        }
    }

    public async Task SoftDeleteApplicationAsync(int id)
    {
        var response = await _httpClient.PatchAsync($"/api/applications/{id}/delete", null);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageAsync(response);
            throw new Exception($"Error soft deleting application: {errorMessage}");
        }
    }

    public async Task RestoreApplicationAsync(int id)
    {
        var response = await _httpClient.PatchAsync($"/api/applications/{id}/restore", null);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageAsync(response);
            throw new Exception($"Error restoring application: {errorMessage}");
        }
    }

    public async Task DeleteApplicationAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/applications/{id}");
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await GetErrorMessageAsync(response);
            throw new Exception($"Error permanently deleting application: {errorMessage}");
        }
    }

    private static Dictionary<string, string> GetQueryParameters(
        QueryParameters? parameters = null)
    {
        parameters ??= new QueryParameters();
        var pageIndex = parameters.PageIndex;
        var pageSize = parameters.PageSize;
        var searchTerm = parameters.SearchTerm;
        var sortBy = parameters.SortBy ?? "date-desc";
        var statusFilters = parameters.StatusFilters != null && parameters.StatusFilters.Length > 0
            ? string.Join(",", parameters.StatusFilters)
            : null;
        var priorityFilters = parameters.PriorityFilters != null && parameters.PriorityFilters.Length > 0
            ? string.Join(",", parameters.PriorityFilters)
            : null;

        var queryParameters = new Dictionary<string, string>
        {
            { "pageIndex", pageIndex.ToString() },
            { "pageSize", pageSize.ToString() },
            { "searchTerm", searchTerm },
            { "sortBy", sortBy },
            { "statusFilters", statusFilters ?? string.Empty },
            { "priorityFilters", priorityFilters ?? string.Empty }
        };
        return queryParameters;
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

public class QueryParameters
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 6;
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; } = "date-desc";
    public int[]? StatusFilters { get; set; }
    public int[]? PriorityFilters { get; set; }
}