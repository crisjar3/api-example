
using NetForemost.API.Attributes;
using System;

namespace NetForemost.API.Requests.TalentPool;
public class GetTalentPoolRequest
{
    public int[] SkillsId { get; set; }
    public int[] JobRolesId { get; set; }
    public int[] SenioritiesId { get; set; }
    public int[] Cities { get; set; }
    public int[] Countries { get; set; }
    public string Email { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? StartRegistrationDate { get; set; }
    public DateTime? EndRegistrationDate { get; set; }
    public string Name { get; set; }
    [IsValidLanguagesAttribute]
    public string Languages { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PerPage { get; set; } = 10;
}
