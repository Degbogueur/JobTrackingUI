namespace JobTrackingUI.PageModels;

public class UpdateApplicationStatusModel
{
    public int Status { get; set; }
    public int? RejectionReason { get; set; }
    public int NextAction { get; set; }
    public DateTime? NextActionDate { get; set; }
}
