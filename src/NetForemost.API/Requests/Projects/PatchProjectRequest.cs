using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.JsonPatch.Operations;
using NetForemost.Core.Entities.Projects;

namespace NetForemost.API.Requests.Projects;

public class PatchProjectRequest
{
    [Required]
    public int ProjectId { get; set; }
    public List<Operation<Project>> PatchDocument { get; set; }
}