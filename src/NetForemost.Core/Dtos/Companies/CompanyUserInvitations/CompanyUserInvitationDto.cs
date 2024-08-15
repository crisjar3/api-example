namespace NetForemost.Core.Dtos.Companies;

public class CompanyUserInvitationDto
{
    public int Id { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string EmailInvited { get; set; }
    public bool IsAccepted { get; set; }
    public bool IsValid { get; set; }
}
