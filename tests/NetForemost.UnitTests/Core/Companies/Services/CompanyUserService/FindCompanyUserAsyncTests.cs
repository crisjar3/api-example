using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Companies.Services.CompanyUserService
{
    public class FindCompanyUserAsyncTests
    {
        /// <summary>
        /// Verify the correct functioning of the entire process of Find CompanyUser
        /// </summary>
        /// <returns> Return Success if all is correct </returns>
        [Fact]
        public async Task WhenFindCompanyUserAsyncIsCorrect_ReturnSuccess()
        {
            //Delcarations of variables
            var userId = "759746bb-df41-4bd1-82d0-459b519c883f";
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

            //Create the simulated service
            var companyUserService = ServiceUtilities.CreateCompanyUserService(
                out _, out _, out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out _, out _, out _, out _, out _);

            //Configurations for tests
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
                It.IsAny<GetCompanyUserIdByUserAndCompanySpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(companyUser);

            var result = await companyUserService.FindCompanyUserAsync(
                userId, companyId
                );

            //Validations for tests
            Assert.True(result.IsSuccess);
        }

        /// <summary>
        /// Verify the correct functioning of the entire process of Find CompanyUser
        /// </summary>
        /// <returns> Return Success if all is correct </returns>
        [Fact]
        public async Task WhenFindCompanyUserAsyncCompanyUserNotFound_ReturnSuccess()
        {
            //Delcarations of variables
            var userId = "759746bb-df41-4bd1-82d0-459b519c883f";
            var companyId = 1;
            var user = new User() { Id = userId, };
            var company = new Company() { Id = companyId, Name = "Company Test" };

            //Create the simulated service
            var companyUserService = ServiceUtilities.CreateCompanyUserService(
                out _, out _, out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out _, out _, out _, out _, out _);

            //Configurations for tests
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
                It.IsAny<GetCompanyUserIdByUserAndCompanySpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync((NetForemost.Core.Entities.Companies.CompanyUser)null);

            var result = await companyUserService.FindCompanyUserAsync(
                userId, companyId
                );

            //Validations for tests
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyUserNotFound);
        }
    }
}
