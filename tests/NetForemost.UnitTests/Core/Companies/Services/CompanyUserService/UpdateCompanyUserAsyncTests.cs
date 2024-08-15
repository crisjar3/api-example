using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Roles;
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
    public class UpdateCompanyUserAsyncTests
    {
        /// <summary>
        /// Verify the correct functioning of the entire process of Update a user company
        /// </summary>
        /// <returns> Return Success if all is correct </returns>
        [Fact]
        public async Task WhenUpdateTeamMemberAsyncIsCorrect_ReturnSuccess()
        {
            int companyId = 1;
            string userIdToUpdate = "759746bb-df41-4bd1-82d0-459b519c883f";
            string userId = "759746bb-df41-4bd1-82d0-459b519c883f";
            int TimeZoneId = 7;
            List<int> listProyectId = new List<int>() { 1, 2 };
            int jobRoleId = 1;
            string roleId = "1";

            var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone()
            {
                Id = TimeZoneId,
                Offset = -6,
                Text = "(UTC-6:00) America/Managua"
            };

            var user = new User() { Id = userId, IsActive = true, TimeZone = timeZone, TimeZoneId = timeZone.Id };

            var userUpdate = new User()
            {
                Id = userIdToUpdate,
                IsActive = true,
                TimeZoneId = timeZone.Id,
                TimeZone = timeZone
            };

            var company = new Company()
            {
                Id = companyId,
                Name = "NetForemost, Inc"
            };
            var role = new Role()
            {
                Id = roleId
            };

            var jobRole = new JobRole()
            {
                Id = jobRoleId,
                Name = "DevOps",
                Description = ""
            };

            var companyUser = new CompanyUser()
            {
                Id = 1,
                UserName = "UserName",
                User = userUpdate,
                UserId = userUpdate.Id,
                Company = company,
                CompanyId = company.Id,
                Role = role,
                RoleId = roleId,
                JobRole = jobRole,
                JobRoleId = jobRoleId,
                TimeZone = timeZone,
                TimeZoneId = TimeZoneId,
                IsActive = true
            };

            var companiesUser = new List<CompanyUser>() { companyUser };
            var project = new Project()
            {
                Id = 1,
                Name = "Project 1",
                Description = "",
                CompanyId = companyId,
                Company = company
            };

            var projectTwo = new Project()
            {
                Id = 2,
                Name = "Project 2",
                Description = "",
                CompanyId = companyId,
                Company = company
            };

            var companiProyectUser = new ProjectCompanyUser()
            {
                Id = 1,
                CompanyUserId = companyUser.Id,
                ProjectId = listProyectId[0],
                JobRoleId = jobRole.Id,
                JobRole = jobRole,
                Project = project
            };

            var companiProyectUserTwo = new ProjectCompanyUser()
            {
                Id = 2,
                CompanyUserId = companyUser.Id,
                ProjectId = listProyectId[1],
                JobRoleId = jobRole.Id,
                JobRole = jobRole,
                Project = projectTwo
            };

            var projectCompanyUserRepositories = new List<ProjectCompanyUser>
            {
                companiProyectUser,
                companiProyectUserTwo
            };

            //Create the simulated service
            var companyUserService = ServiceUtilities.UpdateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository
            );

            //Configurations for tests
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(companyUser);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(true);

            timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(timeZone);

            roleManager.Setup(roleManager => roleManager.FindByIdAsync(
                It.IsAny<string>()
                )).ReturnsAsync(role);

            jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(jobRole);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
                It.IsAny<GetCompanyUserIdByUserAndCompanySpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(companyUser);

            projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.ListAsync(
                It.IsAny<GetProjectCompanyUserByCompanyUserId>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(projectCompanyUserRepositories);

            var result = await companyUserService.UpdateCompanyUserAsync(companyUser, userId);

            //Validations for tests
            Assert.True(result.IsSuccess);
        }

        /// <summary>
        /// Verify the correct functioning of the entire process of Update a user company
        /// </summary>
        /// <returns> Return Success if company user Not Found </returns>

        [Fact]
        public async Task WhenUpdateTeamMemberAsyncCompanyUserNotFound_ReturnSuccess()
        {
            string userIdToUpdate = "759746bb-df41-4bd1-82d0-459b519c883f";
            string userId = "759746bb-df41-4bd1-82d0-459b519c883f";
            int TimeZoneId = 7;
            List<int> listProyectId = new List<int>() { 1, 2 };
            int jobRoleId = 1;
            string roleId = "1";

            var companyUser = new CompanyUser()
            {
                Id = 1,
                UserName = "UserName",
                RoleId = roleId,
                JobRoleId = jobRoleId,
                TimeZoneId = TimeZoneId,
                IsActive = true
            };

            var userIdNotExist = "Not exist";

            //Create the simulated service
            var companyUserService = ServiceUtilities.UpdateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository
            );

            //Configurations for tests
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync((CompanyUser)null);

            var result = await companyUserService.UpdateCompanyUserAsync(companyUser, userId);

            //Validations for tests
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyUserNotFound);
        }

        /// <summary>
        /// Verify the correct functioning of the entire process of Update a user company
        /// </summary>
        /// <returns> Return Success if user do not belong to the company </returns>
        [Fact]
        public async Task WhenUpdateTeamMemberAsyncUserDoNotBelongToTheCompany_ReturnSuccess()
        {
            int companyId = 1;
            string userIdToUpdate = "759746bb-df41-4bd1-82d0-459b519c883f";
            string userId = "759746bb-df41-4bd1-82d0-459b519c883f";
            int TimeZoneId = 7;
            string UserName = "nestorgonzalez";
            List<int> listProyectId = new List<int>() { 1, 2 };
            int jobRoleId = 1;
            string roleId = "1";

            var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone()
            {
                Id = TimeZoneId,
                Offset = -6,
                Text = "(UTC-6:00) America/Managua"
            };

            var user = new User() { Id = userId, IsActive = true, TimeZone = timeZone, TimeZoneId = timeZone.Id };

            var userUpdate = new User()
            {
                Id = userIdToUpdate,
                IsActive = true,
                TimeZoneId = timeZone.Id,
                TimeZone = timeZone
            };

            var company = new Company()
            {
                Id = companyId,
                Name = "NetForemost, Inc"
            };
            var role = new Role()
            {
                Id = roleId
            };

            var jobRole = new JobRole()
            {
                Id = jobRoleId,
                Name = "DevOps",
                Description = ""
            };

            var companyUser = new CompanyUser()
            {
                Id = 1,
                UserName = "UserName",
                User = userUpdate,
                UserId = userUpdate.Id,
                Company = company,
                CompanyId = company.Id,
                Role = role,
                RoleId = roleId,
                JobRole = jobRole,
                JobRoleId = jobRoleId,
                TimeZone = timeZone,
                TimeZoneId = TimeZoneId,
                IsActive = true
            };

            //Create the simulated service
            var companyUserService = ServiceUtilities.UpdateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository
            );

            //Configurations for tests
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(companyUser);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(false);

            var result = await companyUserService.UpdateCompanyUserAsync(companyUser, userId);

            //Validations for tests
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserDoesNotBelongToTheCompany);
        }

        /// <summary>
        /// Verify the correct functioning of the entire process of Update a user company
        /// </summary>
        /// <returns> Return Success if timezone Not Found </returns>
        [Fact]
        public async Task WhenUpdateTeamMemberAsyncTimeZoneNotFound_ReturnSuccess()
        {
            int companyId = 1;
            string userIdToUpdate = "759746bb-df41-4bd1-82d0-459b519c883f";
            string userId = "759746bb-df41-4bd1-82d0-459b519c883f";
            int TimeZoneId = 7;
            List<int> listProyectId = new List<int>() { 1, 2 };
            int jobRoleId = 1;
            string roleId = "1";

            var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone()
            {
                Id = TimeZoneId,
                Offset = -6,
                Text = "(UTC-6:00) America/Managua"
            };

            var user = new User() { Id = userId, IsActive = true, TimeZone = timeZone, TimeZoneId = timeZone.Id };

            var userUpdate = new User()
            {
                Id = userIdToUpdate,
                IsActive = true,
                TimeZoneId = timeZone.Id,
                TimeZone = timeZone
            };

            var company = new Company()
            {
                Id = companyId,
                Name = "NetForemost, Inc"
            };
            var role = new Role()
            {
                Id = roleId
            };

            var jobRole = new JobRole()
            {
                Id = jobRoleId,
                Name = "DevOps",
                Description = ""
            };

            var companyUser = new CompanyUser()
            {
                Id = 1,
                UserName = "UserName",
                User = userUpdate,
                UserId = userUpdate.Id,
                Company = company,
                CompanyId = company.Id,
                Role = role,
                RoleId = roleId,
                JobRole = jobRole,
                JobRoleId = jobRoleId,
                TimeZone = timeZone,
                TimeZoneId = TimeZoneId,
                IsActive = true
            };

            //Create the simulated service
            var companyUserService = ServiceUtilities.UpdateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository
            );

            //Configurations for tests
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(companyUser);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(true);


            timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync((NetForemost.Core.Entities.TimeZones.TimeZone)null);

            var result = await companyUserService.UpdateCompanyUserAsync(companyUser, userId);

            //Validations for tests
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.TimeZoneNotExist);
        }

        /// <summary>
        /// Verify the correct functioning of the entire process of Update a user company
        /// </summary>
        /// <returns> Return Success if Role not found </returns>
        [Fact]
        public async Task WhenUpdateTeamMemberAsyncRoleNotFound_ReturnSuccess()
        {
            int companyId = 1;
            string userIdToUpdate = "759746bb-df41-4bd1-82d0-459b519c883f";
            string userId = "759746bb-df41-4bd1-82d0-459b519c883f";
            int TimeZoneId = 7;
            List<int> listProyectId = new List<int>() { 1, 2 };
            int jobRoleId = 1;
            string roleId = "1";

            var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone()
            {
                Id = TimeZoneId,
                Offset = -6,
                Text = "(UTC-6:00) America/Managua"
            };

            var user = new User() { Id = userId, IsActive = true, TimeZone = timeZone, TimeZoneId = timeZone.Id };

            var userUpdate = new User()
            {
                Id = userIdToUpdate,
                IsActive = true,
                TimeZoneId = timeZone.Id,
                TimeZone = timeZone
            };

            var company = new Company()
            {
                Id = companyId,
                Name = "NetForemost, Inc"
            };
            var role = new Role()
            {
                Id = roleId
            };

            var jobRole = new JobRole()
            {
                Id = jobRoleId,
                Name = "DevOps",
                Description = ""
            };

            var companyUser = new CompanyUser()
            {
                Id = 1,
                UserName = "UserName",
                User = userUpdate,
                UserId = userUpdate.Id,
                Company = company,
                CompanyId = company.Id,
                Role = role,
                RoleId = roleId,
                JobRole = jobRole,
                JobRoleId = jobRoleId,
                TimeZone = timeZone,
                TimeZoneId = TimeZoneId,
                IsActive = true
            };

            //Create the simulated service
            var companyUserService = ServiceUtilities.UpdateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository
            );

            //Configurations for tests
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(companyUser);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(true);

            timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(timeZone);

            roleManager.Setup(roleManager => roleManager.FindByIdAsync(
                It.IsAny<string>()
                )).ReturnsAsync((Role)null);

            var result = await companyUserService.UpdateCompanyUserAsync(companyUser, userId);

            //Validations for tests
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.RoleNotFound.Replace("[id]", roleId.ToString()));
        }

        /// <summary>
        /// Verify the correct functioning of the entire process of Update a user company
        /// </summary>
        /// <returns> Return Success if JobRole Not Found </returns>
        [Fact]
        public async Task WhenUpdateTeamMemberAsyncJobRoleNotFound_ReturnSuccess()
        {
            int companyId = 1;
            string userIdToUpdate = "759746bb-df41-4bd1-82d0-459b519c883f";
            string userId = "759746bb-df41-4bd1-82d0-459b519c883f";
            int TimeZoneId = 7;
            List<int> listProyectId = new List<int>() { 1, 2 };
            int jobRoleId = 1;
            string roleId = "1";

            var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone()
            {
                Id = TimeZoneId,
                Offset = -6,
                Text = "(UTC-6:00) America/Managua"
            };

            var user = new User() { Id = userId, IsActive = true, TimeZone = timeZone, TimeZoneId = timeZone.Id };

            var userUpdate = new User()
            {
                Id = userIdToUpdate,
                IsActive = true,
                TimeZoneId = timeZone.Id,
                TimeZone = timeZone
            };

            var company = new Company()
            {
                Id = companyId,
                Name = "NetForemost, Inc"
            };
            var role = new Role()
            {
                Id = roleId
            };

            var jobRole = new JobRole()
            {
                Id = jobRoleId,
                Name = "DevOps",
                Description = ""
            };

            var companyUser = new CompanyUser()
            {
                Id = 1,
                UserName = "UserName",
                User = userUpdate,
                UserId = userUpdate.Id,
                Company = company,
                CompanyId = company.Id,
                Role = role,
                RoleId = roleId,
                JobRole = jobRole,
                JobRoleId = jobRoleId,
                TimeZone = timeZone,
                TimeZoneId = TimeZoneId,
                IsActive = true
            };

            //Create the simulated service
            var companyUserService = ServiceUtilities.UpdateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository
            );

            //Configurations for tests
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(companyUser);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(true);

            timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(timeZone);

            roleManager.Setup(roleManager => roleManager.FindByIdAsync(
                It.IsAny<string>()
                )).ReturnsAsync(role);

            jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync((JobRole)null);

            var result = await companyUserService.UpdateCompanyUserAsync(companyUser, userId);

            //Validations for tests
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.JobRoleIdNotFound.Replace("[id]", roleId.ToString()));
        }
    }
}
