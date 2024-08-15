using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetForemost.UnitTests.Core.JobRoles.Services
{
    public class GetJobRolesAsync_Tests
    {
        /// <summary>
        /// Verify the correct functioning of the entire process of Get all JobRoles
        /// </summary>
        /// <returns> Return Success if all is correct </returns>
        [Fact]
        public async Task GetJobRolesAsyncIsCorrect_ReturnSuccess()
        {
            //Declarations of variables
            var user = new User() { Id = "1" };
            var company = new Company() { Id = 1 };
            var companyUser = new CompanyUser() { Id = 1, UserId = user.Id, CompanyId = company.Id };

            //Create the simulated service
            var jobRoleService = ServiceUtilities.CreateJobRoleService(
                out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
                out Mock<IAsyncRepository<Company>> companyRepository, out _,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository
                );

            //Configurations for tests
            companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(company);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(true);

            var result = await jobRoleService.GetJobRolesAsync(companyUser.Id, user.Id);

            //Validations for tests
            Assert.True(result.IsSuccess);
        }

        /// <summary>
        /// Verify the correct functioning of the entire process of Get all JobRoles
        /// </summary>
        /// <returns> Return Success if Company Not Found </returns>
        [Fact]
        public async Task GetJobRolesAsyncCompanyNotFound_ReturnSuccess()
        {
            //Declarations of variables
            var user = new User() { Id = "1" };
            var company = new Company() { Id = 1 };
            var companyUser = new CompanyUser() { Id = 1, UserId = user.Id, CompanyId = company.Id };

            //Create the simulated service
            var jobRoleService = ServiceUtilities.CreateJobRoleService(
                out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
                out Mock<IAsyncRepository<Company>> companyRepository, out _,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository
                );

            //Configurations for tests
            companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync((Company)null);

            var result = await jobRoleService.GetJobRolesAsync(companyUser.Id, user.Id);

            //Validations for tests
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyNotFound);
        }

        /// <summary>
        /// Verify the correct functioning of the entire process of Get all JobRoles
        /// </summary>
        /// <returns> Return Success if User Does Not Belong To The Company </returns>
        [Fact]
        public async Task GetJobRolesAsyncUserDoesNotBelongToTheCompany_ReturnSuccess()
        {
            //Declarations of variables
            var user = new User() { Id = "1" };
            var company = new Company() { Id = 1 };
            var companyUser = new CompanyUser() { Id = 1, UserId = user.Id, CompanyId = company.Id };

            //Create the simulated service
            var jobRoleService = ServiceUtilities.CreateJobRoleService(
                out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
                out Mock<IAsyncRepository<Company>> companyRepository, out _,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository
                );

            //Configurations for tests
            companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(company);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(false);

            var result = await jobRoleService.GetJobRolesAsync(companyUser.Id, user.Id);

            //Validations for tests
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserDoesNotBelongToTheCompany);
        }
    }
}
