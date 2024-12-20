namespace ShredKernel.BaseClasses;

public class SearchBaseModel
{
    public string SortBy { get; set; } = "Id";
    
    public bool IsAscending { get; set; } = true;
    
    public string? KeySearch { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }

    public SearchBaseModel(string sortBy, bool? isAscending, string? keySearch, DateTime? startDate, DateTime? endDate)
    {
        this.SortBy = sortBy;
        this.IsAscending = isAscending ?? true;
        this.KeySearch = keySearch;
        this.StartDate = startDate;
        this.EndDate = endDate;
    }
}