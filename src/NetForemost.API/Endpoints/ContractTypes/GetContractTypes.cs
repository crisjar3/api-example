using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetForemost.API.Requests.ContractTypes;
using NetForemost.Core.Dtos.ContractTypes;
using NetForemost.Core.Entities.ContractTypes;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.ContractTypes;
using NetForemost.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetForemost.API.Endpoints.ContractTypes;

public class GetContractTypes : EndpointBaseAsync.WithRequest<GetContractTypeRequest>.WithActionResult<List<ContractTypeDto>>
{
    private readonly IContractTypeService _contractTypeService;
    private readonly ILogger<GetContractTypes> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetContractTypes(UserManager<User> userManager, IMapper mapper, ILogger<GetContractTypes> logger, IContractTypeService contractTypeService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _contractTypeService = contractTypeService;
    }

    [ProducesResponseType(200, Type = typeof(List<ContractTypeDto>))]
    [ProducesResponseType(400, Type = typeof(ProblemDetails))]
    [ProducesResponseType(500, Type = typeof(ProblemDetails))]
    [HttpGet(Routes.ContractType.GetContractTypes)]
    [SwaggerOperation(
        Summary = "Get all contract types.",
        Description = "Get all contract types",
        OperationId = "ContractType.GetContractTypes",
        Tags = new[] { "ContractType" })
    ]
    [Authorize]
    public override async Task<ActionResult<List<ContractTypeDto>>> HandleAsync([FromQuery] GetContractTypeRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.GetUserAsync(User);
        try
        {
            _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.ContractType.GetContractTypes, request, user.Email));

            var result = await _contractTypeService.FindContractTypesAsync(request.CompanyId);

            var mapped = _mapper.Map<List<ContractType>, List<ContractTypeDto>>(result.Value);

            _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.ContractType.GetContractTypes, request, user.Email));

            return Ok(mapped);
        }
        catch (Exception ex)
        {
            var error = ErrorHelper.GetExceptionError(ex);

            _logger.LogError(LoggerHelper.EndpointRequestError(Routes.ContractType.GetContractTypes, request, error, user.Email));

            return Problem(error, Routes.ContractType.GetContractTypes, 500);
        }
    }
}