using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.Core.Specifications.Projects;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Companies.Services.CompanyUserService
{
    public class GetCompanyUserDetailsAsyncTests
    {
        /// <summary>
        /// Verify the correct functioning of the entire process of Find Member
        /// </summary>
        /// <returns> Return Success if all is correct </returns>
        [Fact]
        public async Task WhenFindCompanyUserDetailsAsyncIsCorrect_ReturnSuccess()
        {
            //Delcarations of variables
            var userId = "1";
            var companyUserId = 1;
            var companyId = 1;
            var user = new User() { Id = userId, };
            var company = new Company() { Id = companyId, Name = "Company Test" };

            var companyUser = new CompanyUser()
            {
                Id = 1,
                User = user,
                UserId = user.Id,
                Company = company,
                CompanyId = company.Id,
                IsActive = true
            };

            var companiesUser = new List<CompanyUser>() { companyUser };

            //Create the simulated service
            var companyUserService = ServiceUtilities.CreateCompanyUserService(
                out _, out _, out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out _, out _, out _, out _, out Mock<IAsyncRepository<Project>> projectRepository);

            //Configurations for tests
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
                It.IsAny<GetCompanyUserByCompanyUserIdSpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(companyUser);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(true);

            projectRepository.Setup(projectRepository => projectRepository.ListAsync(
                It.IsAny<GetProjectsByCompanyIdSpecification>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(new List<Project>() { new Project() { Id = 1,CompanyId=company.Id, Name = "New project" } });

            var result = await companyUserService.GetCompanyUserDetailsAsync(userId, companyUserId);

            //Validations for tests
            Assert.True(result.IsSuccess);
        }

        /// <summary>
        /// Verify the correct functioning of the entire process of Company User
        /// </summary>
        /// <returns> Return Success if Company User Not Found </returns>
        [Fact]
        public async Task WhenFindCompanyUserDetailsCompanyUserNotFound_ReturnSuccess()
        {
            //Delcarations of variables
            var userId = "1";
            var companyUserId = 1;
            var companyId = 1;
            var user = new User() { Id = userId, };
            var company = new Company() { Id = companyId, Name = "Company Test" };

            var companyUser = new CompanyUser()
            {
                Id = 1,
                User = user,
                UserId = user.Id,
                Company = company,
                CompanyId = company.Id,
                IsActive = true
            };

            var companiesUser = new List<CompanyUser>() { companyUser };

            //Create the simulated service
            var companyUserService = ServiceUtilities.CreateCompanyUserService(
                out _, out _, out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out _, out _, out _, out _, out _);

            //Configurations for tests
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
                It.IsAny<GetCompanyUserByCompanyUserIdSpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync((CompanyUser) null);

            var result = await companyUserService.GetCompanyUserDetailsAsync(userId, companyUserId);

            //Validations for tests
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyUserNotFound);
        }

        /// <summary>
        /// Verify the correct functioning of the entire process of CompanyUser
        /// </summary>
        /// <returns> Return Success if User does not belong to the company </returns>
        [Fact]
        public async Task WhenFindCompanyUserDetailsAsyncUserDoesNotBelongToTheCompany_ReturnSuccess()
        {
            //Delcarations of variables
            var userId = "1";
            var companyUserId = 1;
            var companyId = 1;
            var user = new User() { Id = userId, };
            var company = new Company() { Id = companyId, Name = "Company Test" };

            var companyUser = new CompanyUser()
            {
                Id = 1,
                User = user,
                UserId = user.Id,
                Company = company,
                CompanyId = company.Id,
                IsActive = true
            };

            var companiesUser = new List<CompanyUser>() { companyUser };

            //Create the simulated service
            var companyUserService = ServiceUtilities.CreateCompanyUserService(
                out _, out _, out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out _, out _, out _, out _, out _);

            //Configurations for tests
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
                It.IsAny<GetCompanyUserByCompanyUserIdSpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(companyUser);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(false);

            var result = await companyUserService.GetCompanyUserDetailsAsync(userId, companyUserId);

            //Validations for tests
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserDoesNotBelongToTheCompany);
        }

    }
}
