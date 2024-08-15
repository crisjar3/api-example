using NetForemost.SharedKernel.Entities;

namespace NetForemost.API.Requests.Tasks
{
    public class GetTaskTypesRequest : PaginationRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int ProjectId { get; set; }
        public int CompanyId { get; set; }

    }
}
