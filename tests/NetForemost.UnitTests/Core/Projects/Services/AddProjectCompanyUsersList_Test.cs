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

namespace NetForemost.UnitTests.Core.Projects.Services
{
    public class AddProjectCompanyUsersList_Test
    {
        /// <summary>
        /// Check if all proccess is correct
        /// </summary>
        /// <returns>Success, when all proccess is correct</returns>
        [Fact]
        public async Task WhenAllIsCorrect_ReturnSuccess()
        {
            //Declarate Variables
            var companyUser = new CompanyUser() { Id = 1, CompanyId = 1, UserId = "1" };
            int[] projectIds = { 1, 2, 3 };
            var project = new Project() { Id = 1 };
            User user = new() { Id = "1" };
            var projectCompanyUserList = new List<ProjectCompanyUser>()
            {
                new ProjectCompanyUser(){Id=1,CompanyUserId=1,ProjectId=1},
                new ProjectCompanyUser(){Id=2,CompanyUserId=1,ProjectId=2},
                new ProjectCompanyUser(){Id=3,CompanyUserId=1,ProjectId=3},
            };

            //Create service
            var projectService = ServiceUtilities.CreateProjectServices(
                out _,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out Mock<IAsyncRepository<Project>> projectRepository,
                out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
                out _, out _,
                out _, out _);

            //Configuration test
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(companyUser);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(true);

            projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.ListAsync(
                It.IsAny<GetProjectCompanyUserByCompanyUserId>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(projectCompanyUserList);

            projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(project);

            var result = await projectService.AddProjectCompanyUsersList(companyUser.Id, projectIds, user.Id);

            //Validation Test
            Assert.True(result.IsSuccess);
        }

        /// <summary>
        /// Check if all proccess is correct
        /// </summary>
        /// <returns>Success, when Company User Not Found</returns>
        [Fact]
        public async Task WhenCompanyUserNotFound_ReturnSuccess()
        {
            //Declarate Variables
            var companyUser = new CompanyUser() { Id = 1, CompanyId = 1, UserId = "1" };
            int[] projectIds = { 1, 2, 3 };
            var project = new Project() { Id = 1 };
            User user = new() { Id = "1" };
            var projectCompanyUserList = new List<ProjectCompanyUser>()
            {
                new ProjectCompanyUser(){Id=1,CompanyUserId=1,ProjectId=1},
                new ProjectCompanyUser(){Id=2,CompanyUserId=1,ProjectId=2},
                new ProjectCompanyUser(){Id=3,CompanyUserId=1,ProjectId=3},
            };

            //Create service
            var projectService = ServiceUtilities.CreateProjectServices(
                out _,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out Mock<IAsyncRepository<Project>> projectRepository,
                out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
                out _,out _,
                out _, out _);

            //Configuration test
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())).ReturnsAsync((CompanyUser)null);

            var result = await projectService.AddProjectCompanyUsersList(companyUser.Id, projectIds, user.Id);

            //Validation Test
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyUserNotFound);
        }

        /// <summary>
        /// Check if all proccess is correct
        /// </summary>
        /// <returns>Success, when User Don Not Belong to the Company</returns>
        [Fact]
        public async Task WhenUserDonNotBelongToTheCompany_ReturnSuccess()
        {
            //Declarate Variables
            var companyUser = new CompanyUser() { Id = 1, CompanyId = 1, UserId = "1" };
            int[] projectIds = { 1, 2, 3 };
            var project = new Project() { Id = 1 };
            User user = new() { Id = "1" };
            var projectCompanyUserList = new List<ProjectCompanyUser>()
            {
                new ProjectCompanyUser(){Id=1,CompanyUserId=1,ProjectId=1},
                new ProjectCompanyUser(){Id=2,CompanyUserId=1,ProjectId=2},
                new ProjectCompanyUser(){Id=3,CompanyUserId=1,ProjectId=3},
            };

            //Create service
            var projectService = ServiceUtilities.CreateProjectServices(
                out _,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out Mock<IAsyncRepository<Project>> projectRepository,
                out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
                out _, out _,
                out _, out _);

            //Configuration test
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(companyUser);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(false);

            var result = await projectService.AddProjectCompanyUsersList(companyUser.Id, projectIds, user.Id);

            //Validation Test
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserDoesNotBelongToTheCompany);
        }

        /// <summary>
        /// Check if all proccess is correct
        /// </summary>
        /// <returns>Success, when all Project not Found</returns>
        [Fact]
        public async Task WhenProjectNotFound_ReturnSuccess()
        {
            //Declarate Variables
            var companyUser = new CompanyUser() { Id = 1, CompanyId = 1, UserId = "1" };
            int[] projectIds = { 1, 2, 3 };
            var project = new Project() { Id = 3 };
            User user = new() { Id = "1" };
            var projectCompanyUserList = new List<ProjectCompanyUser>()
            {
                new ProjectCompanyUser(){Id=1,CompanyUserId=1,ProjectId=1},
                new ProjectCompanyUser(){Id=2,CompanyUserId=1,ProjectId=2}
            };

            //Create service
            var projectService = ServiceUtilities.CreateProjectServices(
                out _,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out Mock<IAsyncRepository<Project>> projectRepository,
                out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
                out _, out _,
                out _, out _);

            //Configuration test
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(companyUser);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(true);

            projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.ListAsync(
                It.IsAny<GetProjectCompanyUserByCompanyUserId>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(projectCompanyUserList);

            projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
                It.IsAny<GetProjectCompanyUserByCompanyUserId>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync((Project)null);

            var result = await projectService.AddProjectCompanyUsersList(companyUser.Id, projectIds, user.Id);

            //Validation Test
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.ProjectNotFound.Replace("[id]", project.Id.ToString()));
        }

        /// <summary>
        /// Check if all proccess is correct
        /// </summary>
        /// <returns>Success, when Project do not owned to the company</returns>
        [Fact]
        public async Task WhenProjectDoNotOwnedToTheCompany_ReturnSuccess()
        {
            //Declarate Variables
            var companyUser = new CompanyUser() { Id = 1, CompanyId = 1, UserId = "2" };
            int[] projectIds = { 1, 2, 3 };
            var project = new Project() { Id = 3, CompanyId = 2 };
            User user = new() { Id = "2" };
            var projectCompanyUserList = new List<ProjectCompanyUser>()
            {
                new ProjectCompanyUser(){Id=1,CompanyUserId=1,ProjectId=1},
                new ProjectCompanyUser(){Id=2,CompanyUserId=1,ProjectId=2},
            };

            //Create service
            var projectService = ServiceUtilities.CreateProjectServices(
                out _,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out Mock<IAsyncRepository<Project>> projectRepository,
                out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
                out _, out _,
                out _, out _);

            //Configuration test
            companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(companyUser);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(true);

            projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.ListAsync(
                It.IsAny<GetProjectCompanyUserByCompanyUserId>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(projectCompanyUserList);

            projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(project);

            var result = await projectService.AddProjectCompanyUsersList(companyUser.Id, projectIds, user.Id);

            //Validation Test
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.ProjectNotOwnedTheCompany);
        }
    }
}
