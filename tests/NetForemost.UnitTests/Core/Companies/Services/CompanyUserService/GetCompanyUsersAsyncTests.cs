using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Companies.Services.CompanyUserService
{
    public class GetCompanyUsersAsyncTests
    {
        /// <summary>
        /// Verify the correct functioning of the entire process of Get Team Members
        /// </summary>
        /// <returns> Return Success if all is correct </returns>
        [Fact]
        public async Task WhenGetCompanyUsersAsyncIsCorrect_ReturnSuccess()
        {

            var userId = "1";
            var companyId = 1;
            int pageNumber = 1;
            int perPage = 10;

            var user = new User()
            {
                Id = userId,
                IsActive = true,
            };

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

            var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager, out _, out _, out _);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.ListAsync(
            It.IsAny<GetCompanyUserByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(companiesUser);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.CountAsync(
            It.IsAny<GetCompanyUserByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(companiesUser.Count);

            var result = await companyUserService.GetCompanyUsersAsync(userId, companyId, pageNumber, perPage);

            Assert.True(result.IsSuccess);
        }

        /// <summary>
        /// Verify the correct functioning of the entire process of Get Team Members
        /// </summary>
        /// <returns> Return Success if user does not belong to the company </returns>
        [Fact]
        public async Task WhenGetCompanyUsersAsyncUserDoesNotBelongToTheCompany_ReturnSuccess()
        {

            var userId = "1";
            var companyId = 1;
            int pageNumber = 1;
            int perPage = 10;

            var user = new User()
            {
                Id = userId,
                IsActive = true,
            };

            var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager, out _, out _, out _);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

            var result = await companyUserService.GetCompanyUsersAsync(userId, companyId, pageNumber, perPage);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserDoesNotBelongToTheCompany);
        }
    }
}