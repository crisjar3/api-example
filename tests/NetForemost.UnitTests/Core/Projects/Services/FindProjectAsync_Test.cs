using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Projects.Services;

public class FindProjectAsync_Test
{

    /// <summary>
    /// check if UserNobelong in company
    /// </summary>
    /// <returns>Error, User Does Not Belong To The Company</returns>
    [Fact]
    public async Task WhenUserNoBelongCompany_ResultErorrUserDoesNotBelongToTheCompany()
    {
        //Declarate variables
        int projectId = 18;
        string name = "";
        string descripcion = "";
        int companyId = new();
        string[] techStack = new string[10];
        float BudgetRnageFrom = new();
        float BudgetRnageto = new();
        DateTime dateStartTo = new();
        DateTime dateStartFrom = new();
        DateTime dateEndtTo = new();
        DateTime dateEndFrom = new();
        string userId = "";
        string userIdFilter = "";
        int pageNumber = 1;
        int perPage = 10;

        //Created simulate service
        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _,
            out _,
            out _,
            out _,
            out _, out _
            );

        //configuration for test 
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
             It.IsAny<CheckUserIsInCompanySpecification>(),
             It.IsAny<CancellationToken>())).ReturnsAsync(false);


        //Validation test

        var result = await projectService.FindProjectsAsync(projectId, name, descripcion, companyId, techStack,
                                                        BudgetRnageFrom, BudgetRnageto, userId, dateStartTo, dateStartFrom,
                                                        dateEndtTo, dateEndFrom, userIdFilter, pageNumber, perPage);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.UserDoesNotBelongToTheCompany, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// check if all is correct in the proccess
    /// </summary>
    /// <returns>Suceess, when al procces is correct</returns>
    [Fact]
    public async Task WhenAllIsCorrect_ResultIsSuccess()
    {
        //Declarate variables
        string name = "";
        string descripcion = "";
        int companyId = new();
        string[] techStack = new string[10];
        float BudgetRnageFrom = new();
        float BudgetRnageto = new();
        DateTime dateStartTo = new();
        DateTime dateStartFrom = new();
        DateTime dateEndtTo = new();
        DateTime dateEndFrom = new();
        string userId = "";
        string userIdFilter = "";
        int pageNumber = 1;
        int perPage = 10;

        //Created simulate service
        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _,
            out _,
            out _,
            out _,
            out _, out _
            );

        //configuration for test 
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
             It.IsAny<CheckUserIsInCompanySpecification>(),
             It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Validation test

        var result = await projectService.FindProjectsAsync(18, name, descripcion, companyId, techStack,
                                                        BudgetRnageFrom, BudgetRnageto, userId, dateStartTo, dateStartFrom,
                                                        dateEndtTo, dateEndFrom, userIdFilter, pageNumber, perPage);

        Assert.True(result.IsSuccess);
    }


    /// <summary>
    /// check if unexpected error
    /// </summary>
    /// <returns>unexpected error</returns>
    [Fact]
    public async Task WhenUnexpectedError_ResultErorr()
    {
        //Declarate variables
        string name = "";
        string descripcion = "";
        int companyId = new();
        string[] techStack = new string[10];
        float BudgetRnageFrom = new();
        float BudgetRnageto = new();
        DateTime dateStartTo = new();
        DateTime dateStartFrom = new();
        DateTime dateEndtTo = new();
        DateTime dateEndFrom = new();
        string userId = "";
        string userIdFilter = "";
        int pageNumber = 1;
        int perPage = 10;
        var errorMessage = "TESTE ERROR";

        //Created simulate service
        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _,
            out _,
            out _,
            out _,
            out _, out _
            );

        //configuration for test 
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
             It.IsAny<CheckUserIsInCompanySpecification>(),
             It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        //Validation test
        var result = await projectService.FindProjectsAsync(18, name, descripcion, companyId, techStack,
                                                        BudgetRnageFrom, BudgetRnageto, userId, dateStartTo, dateStartFrom,
                                                        dateEndtTo, dateEndFrom, userIdFilter, pageNumber, perPage);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), errorMessage);
    }
}
