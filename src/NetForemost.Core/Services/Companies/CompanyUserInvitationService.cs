using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NetForemost.Core.Dtos.Companies.CompanyUserInvitations;
using NetForemost.Core.Dtos.Projects;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.Core.Interfaces.SendGrid;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.Core.Specifications.Projects;
using NetForemost.Core.Specifications.Users;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Companies;

public class CompanyUserInvitationService : ICompanyUserInvitationService
{
    private readonly IAsyncRepository<CompanyUserInvitation> _companyUserInvitationRepository;
    private readonly IAsyncRepository<CompanyUser> _companyUserRepository;
    private readonly IAsyncRepository<Company> _companyRepository;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAsyncRepository<Project> _projectRepository;
    private readonly ISendGridService _SendGridService;

    public CompanyUserInvitationService(IAsyncRepository<CompanyUserInvitation> companyUserInvitationRepository, IAsyncRepository<CompanyUser> companyUserRepository, IAsyncRepository<Company> companyRepository, RoleManager<Role> roleManager, IAsyncRepository<Project> projectRepository, ISendGridService sendGridService)
    {
        _companyUserInvitationRepository = companyUserInvitationRepository;
        _companyUserRepository = companyUserRepository;
        _companyRepository = companyRepository;
        _roleManager = roleManager;
        _projectRepository = projectRepository;
        _SendGridService = sendGridService;
    }

    public async Task<Result<NoContentResult>> CreateInvitationCompanyUserAsync(CompanyUserInvitation newCompanyUserInvitation, IEnumerable<ProjectDtoSimple> projects, string userId)
    {
        //Get tasks
        var belongProjectsToCompanyTask = ValidateProjectsAsync(projects, newCompanyUserInvitation.CompanyId);
        var existRoleTask = _roleManager.FindByIdAsync(newCompanyUserInvitation.RoleId);
        var emailBelongToCompanyTask = _companyUserRepository.AnyAsync(new ValidateEmailBelongToCompanySpecification(newCompanyUserInvitation.EmailInvited, newCompanyUserInvitation.CompanyId));
        var companyTask = _companyRepository.GetByIdAsync(newCompanyUserInvitation.CompanyId);

        await emailBelongToCompanyTask;
        //Validations of email
        if (emailBelongToCompanyTask.Result)
        {
            return Result.Invalid(new List<ValidationError>()
            {
                new()
                {
                    ErrorMessage = ErrorStrings.EmailExistsInTheCompany
                }
            });
        }

        var company = await companyTask;
        //Validation of company
        if (company is null)
        {
            return Result.Invalid(new List<ValidationError>()
            {
                new()
                {
                    ErrorMessage = ErrorStrings.CompanyNotFound
                }
            });
        }
        await existRoleTask;
        //Validation of Role
        if (existRoleTask.Result is null)
        {
            return Result.Invalid(new List<ValidationError>()
            {
                new()
                {
                    ErrorMessage = ErrorStrings.RoleNotFound
                }
            });
        }

        await belongProjectsToCompanyTask;
        if (!belongProjectsToCompanyTask.Result.IsSuccess)
        {
            return Result.Invalid(belongProjectsToCompanyTask.Result.ValidationErrors);
        }

        //Complete data invitation 
        newCompanyUserInvitation.ExpirationDate = DateTime.Now.AddYears(1);
        newCompanyUserInvitation.InvitationToken = Guid.NewGuid();
        newCompanyUserInvitation.AddCreatedInfo(userId);

        var registerLink = RouteStrings.Url_Invitation
            .Replace("[HostApp]", RouteStrings.Url_register_invited)
            .Replace("[invitation_token]", newCompanyUserInvitation.InvitationToken.ToString());

        var trySendEmail = await _SendGridService.TrySendEmailAsync("Timeforemost",
            HtmlTemplates.EmailWithButton
            .Replace("[MainText]", HtmlTemplatesStrings.Greeting.Replace("[company]", company.Name))
            .Replace("[ButtonText]", HtmlTemplatesStrings.Button_Invitation)
            .Replace("[ClosingText]", HtmlTemplatesStrings.EmailClosingText)
            .Replace("[HelpText]", HtmlTemplatesStrings.EmailHelpText)
            .Replace("[ButtonLink]", registerLink),
            "",
            newCompanyUserInvitation.EmailInvited,
            ""
            );

        if (!trySendEmail)
        {
            return Result.Error(trySendEmail.Errors.ToArray());
        }

        var result = await _companyUserInvitationRepository.AddAsync(newCompanyUserInvitation);

        result.ProjectCompanyUserInvitations = projects.Select(project => new ProjectCompanyUserInvitation()
        {
            ProjectId = project.Id,
            CompanyUserInvitationId = result.Id,
            CreatedAt = DateTime.Now,
            CreatedBy = userId
        }).ToList();

        await _companyUserInvitationRepository.UpdateAsync(result);

        return Result.Success();
    }

    public async Task<Result<PaginatedRecord<CompanyUserInvitationCompleteDto>>> FindInvitationByCompanyAsync(int companyId, int perPage, int pageNumber)
    {
        try
        {
            // verify if company exist
            var existCompany = await _companyRepository.AnyAsync(companyId);

            if (!existCompany)
            {
                return Result.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.CompanyNotFound
                    }
                });
            }

            var count = await _companyUserInvitationRepository.CountAsync(new GetCompanyUserInvitationSpecification(companyId));

            var invitations = await _companyUserInvitationRepository.ListAsync(new GetCompanyUserInvitationSpecification(companyId));

            var paginatedRecords = new PaginatedRecord<CompanyUserInvitationCompleteDto>(invitations, count, perPage, pageNumber);

            return Result.Success(paginatedRecords);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
    private async Task<Result> ValidateProjectsAsync(IEnumerable<ProjectDtoSimple> projects, int companyId)
    {
        var projectsBelong = await _projectRepository.ListAsync(new GetCompanyProjectsSpecification(projects.Select(projects => projects.Id).ToArray(), companyId));

        var projectsNotBelong = projects.Where(project => !projectsBelong.Any(project2 => project2.Id == project.Id));

        if (!projectsNotBelong.IsNullOrEmpty())
        {
            string projectsNotBelongNames = string.Join(", ", projectsNotBelong.Select(project => project.Name));
            return Result.Invalid(new List<ValidationError>()
            {
                new()
                {
                    ErrorMessage = ErrorStrings.ProjectNotBelongToCompany.Replace("[Name]", projectsNotBelongNames)
                }
            });
        }

        return Result.Success();
    }
}