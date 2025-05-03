namespace JobTrackingUI.Helpers;

public record EnumItem(int Id, string Name);

public class AllEnums
{
    public List<EnumItem> ActionTypes { get; set; } = [];
    public List<EnumItem> ApplicationStatuses { get; set; } = [];
    public List<EnumItem> ContractTypes { get; set; } = [];
    public List<EnumItem> Currencies { get; set; } = [];
    public List<EnumItem> JobSources { get; set; } = [];
    public List<EnumItem> Priorities { get; set; } = [];
    public List<EnumItem> RejectionReasons { get; set; } = [];
}