using System.ComponentModel.DataAnnotations;

namespace JobTrackingUI.PageModels;

public class EditApplicationModel
{
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime ApplicationDate { get; set; } = DateTime.Today;

    [Required]
    [StringLength(100, ErrorMessage = "Job title cannot be longer than 100 characters.")]
    public string JobTitle { get; set; }

    public string? JobDescription { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Company name cannot be longer than 100 characters.")]
    public string CompanyName { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Location cannot be longer than 100 characters.")]
    public string Location { get; set; }

    [Required]
    public int Source { get; set; }

    [Required]
    public int ContractType { get; set; }

    [Required]
    public string OfferUrl { get; set; }

    public DateTime? PostingDate { get; set; }

    public DateTime? ClosingDate { get; set; }

    public IFormFile? Resume { get; set; }
    public string ResumeFilePath { get; set; }

    public IFormFile? CoverLetter { get; set; }
    public string? CoverLetterFilePath { get; set; }

    [Required]
    public int Status { get; set; }

    [Required]
    public int LastAction { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime LastActionDate { get; set; }

    [Required]
    public int NextAction { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? NextActionDate { get; set; }

    public int? Priority { get; set; }

    public string? Notes { get; set; }

    public decimal? MinSalaryProposed { get; set; }

    public decimal? MaxSalaryProposed { get; set; }

    public decimal? MinSalaryOffered { get; set; }

    public decimal? MaxSalaryOffered { get; set; }

    public int? Currency { get; set; }

    public int? RejectionReason { get; set; }

    public string? KeyWords { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "Interest level must be between 1 and 5.")]
    public int InterestLevel { get; set; }

    public string? ContactName { get; set; }

    public string? ContactEmail { get; set; }
}
