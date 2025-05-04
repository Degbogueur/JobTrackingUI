using JobTrackingUI.Helpers;
using JobTrackingUI.PageModels;

namespace JobTrackingUI.Mappers;

public static class ApplicationMappers
{
    public static EditApplicationModel ToEditModel(this ApplicationModel model, AllEnums enums)
    {
        return new EditApplicationModel
        {
            ApplicationDate = model.ApplicationDate,
            JobTitle = model.JobTitle,
            JobDescription = model.JobDescription,
            CompanyName = model.CompanyName,
            Location = model.Location,
            Source = enums.JobSources.FirstOrDefault(s => s.Name == model.Source)?.Id ?? 0,
            ContractType = enums.ContractTypes.FirstOrDefault(ct => ct.Name == model.ContractType)?.Id ?? 0,
            OfferUrl = model.OfferUrl,
            PostingDate = model.PostingDate,
            ClosingDate = model.ClosingDate,
            ResumeFilePath = model.ResumeFilePath,
            CoverLetterFilePath = model.CoverLetterFilePath,
            Status = enums.ApplicationStatuses.FirstOrDefault(s => s.Name == model.Status)?.Id ?? 0,
            LastAction = enums.ActionTypes.FirstOrDefault(a => a.Name == model.LastAction)?.Id ?? 0,
            LastActionDate = model.LastActionDate,
            NextAction = enums.ActionTypes.FirstOrDefault(a => a.Name == model.NextAction)?.Id ?? 0,
            NextActionDate = model.NextActionDate,
            Priority = enums.Priorities.FirstOrDefault(p => p.Name == model.Priority)?.Id ?? 0,
            KeyWords = model.KeyWords,
            InterestLevel = model.InterestLevel,
            Currency = enums.Currencies.FirstOrDefault(c => c.Name == model.Currency)?.Id,
            MinSalaryProposed = model.MinSalaryProposed,
            MaxSalaryProposed = model.MaxSalaryProposed,
            MinSalaryOffered = model.MinSalaryOffered,
            MaxSalaryOffered = model.MaxSalaryOffered,
            ContactName = model.ContactName,
            ContactEmail = model.ContactEmail,
            Notes = model.Notes
        };
    }
}
