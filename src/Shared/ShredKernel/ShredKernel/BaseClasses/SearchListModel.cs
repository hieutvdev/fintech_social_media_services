namespace ShredKernel.BaseClasses;

public class SearchListModel
{
    public int? PageIndex { get; set; }

    public int? PageSize { get; set; }
    
    public string? SortBy { get; set; } = "Id";
    
    public bool? IsAscending { get; set; } = true;
    
    public string? KeySearch { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
}