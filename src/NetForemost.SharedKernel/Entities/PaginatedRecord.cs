namespace NetForemost.SharedKernel.Entities;

public class PaginatedRecord<T>
{
    private PaginatedRecord()
    {
    }
    public PaginatedRecord(List<T> records, int totalRecordsCount, int perPage = 10, int pageNumber = 1)
    {
        Records = records;

        PagingInfo.TotalRecords = totalRecordsCount;
        PagingInfo.CurrentPage = pageNumber;
        PagingInfo.PerPage = perPage;
    }

    public List<T> Records { get; set; }
    public PagingInfo PagingInfo { get; private set; } = new PagingInfo();
}

public class PagingInfo
{
    public int PerPage { get; set; }
    public int CurrentPage { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages => (PerPage == 0) ? 0 : (TotalRecords / PerPage) + (TotalRecords % PerPage == 0 ? 0 : 1);

    public bool IsValid()
    {
        return CurrentPage >= 1 && CurrentPage <= TotalPages;
    }
}