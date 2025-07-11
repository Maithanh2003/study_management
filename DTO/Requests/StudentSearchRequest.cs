public class StudentSearchRequest
{
    public string? Keyword { get; set; }
    public string? Gender { get; set; }
    public string? Religion { get; set; }
    public int? ClassId { get; set; }
    public string? SortBy { get; set; } = "FirstName";
    public bool IsDescending { get; set; } = false;

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}
