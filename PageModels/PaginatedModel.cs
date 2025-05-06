namespace JobTrackingUI.PageModels;

public class PaginatedModel<T>
{
    public List<T>? Items { get; set; }
    public int TotalItems { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
