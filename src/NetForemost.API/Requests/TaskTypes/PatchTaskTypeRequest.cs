using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using NetForemost.Core.Entities.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace NetForemost.API.Requests.TaskTypes;

public class PostTaskTypesActivateRequest
{
    public int[] TaskTypesIds { get; set; }
}