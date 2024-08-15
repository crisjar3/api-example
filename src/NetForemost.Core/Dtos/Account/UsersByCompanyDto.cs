namespace NetForemost.Core.Dtos.Account;

public class UsersByCompanyDto
{
    public int CompanyUserId { get; set; }
    public string FullName { get; set; }
    public int CompanyId { get; set; }
    public bool BelongProject { get; set; }
}