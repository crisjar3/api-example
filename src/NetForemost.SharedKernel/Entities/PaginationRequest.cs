using System.ComponentModel.DataAnnotations;

namespace NetForemost.SharedKernel.Entities;

public class PaginationRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "It should be greater than 0")]
    public int PerPage { get; set; } = 100;

    [Range(1, int.MaxValue, ErrorMessage = "It should be greater than 0")]
    public int PageNumber { get; set; } = 1;
}