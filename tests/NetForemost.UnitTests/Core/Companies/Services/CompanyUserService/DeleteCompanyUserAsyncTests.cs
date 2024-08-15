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
    public class DeleteCompanyUserAsyncTests
    {
        /// <summary>
        /// Verify the correct functioning of the entire process of Delete Member
        /// </summary>
        /// <returns> Return Success if all is correct </returns>
        [Fact]
        public async Task WhenDeleteCompanyUserAsyncIsCorrect_ReturnSuccess()
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

            //Create the simulated service
            var companyUserService = ServiceUtilities.CreateCompanyUserService(
                out _, out _, out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out _, out _,out _, out _, out _);

            //Configurations for tests
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(companyUser);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(true);

            var result = await companyUserService.DeleteCompanyUserAsync(userId, companyUserId);

            //Validations for tests
            Assert.True(result.IsSuccess);
        }

        /// <summary>
        /// Verify the correct functioning of the entire process of Delete Member
        /// </summary>
        /// <returns> Return Success if CompanyUser does not found </returns>
        [Fact]
        public async Task WhenDeleteCompanyUserAsyncCompanyUserDoesNotFound_ReturnSuccess()
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

            //Create the simulated service
            var companyUserService = ServiceUtilities.CreateCompanyUserService(
                out _, out _, out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out _, out _, out _, out _, out _);

            //Configurations for tests
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync((CompanyUser)null);

            var result = await companyUserService.DeleteCompanyUserAsync(userId, companyUserId);

            //Validations for tests
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyUserNotFound);
        }

        /// <summary>
        /// Verify the correct functioning of the entire process of Delete Member
        /// </summary>
        /// <returns> Return Success if user does not belong to the company found </returns>
        [Fact]
        public async Task WhenDeleteCompanyUserAsyncUserDoesNotBelongToTheCompany_ReturnSuccess()
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

            //Create the simulated service
            var companyUserService = ServiceUtilities.CreateCompanyUserService(
                out _, out _, out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out _, out _, out _, out _, out _);

            //Configurations for tests
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(companyUser);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(false);

            var result = await companyUserService.DeleteCompanyUserAsync(userId, companyUserId);

            //Validations for tests
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserDoesNotBelongToTheCompany);
        }
    }
}
