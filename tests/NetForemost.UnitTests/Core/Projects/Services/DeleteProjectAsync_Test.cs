using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Projects.Services
{
    public class DeleteProjectAsync_Test
    {
        /// <summary>
        /// check result when Project is deleted
        /// </summary>
        /// <returns>Error, Project is deleted</returns>
        [Fact]
        public async Task WhenDeleteProjectAsyncIsCorrect_ReturnIsCorrect()
        {
            //Declaration Variables
            var project = new Project()
            {
                Id = 1,
                Name = "Project one",
                Description = "",
                CompanyId = 1
            };
            var user = new User()
            {
                Id = "1"
            };
            var companyUser = new CompanyUser()
            {
                Id = 1,
                UserName = "name",
                UserId = "1"
            };


            //Create simulated service
            var projectService = ServiceUtilities.CreateProjectServices(
                out _,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out Mock<IAsyncRepository<Project>> projectRepository,
                out _,
                out _,
                out _,
                out _, out _);

            //configuration Test

            projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
                It.IsAny<int>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(project);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>()
                    )).ReturnsAsync(true);

            //validated test

            var result = await projectService.DeleteProject(user.Id, project.Id);
            Assert.True(result.IsSuccess);
        }

        /// <summary>
        /// check result when Project Not Found
        /// </summary>
        /// <returns>Error, Project Not Found</returns>
        [Fact]
        public async Task WhenDeleteProjectAsyncProjectNotFound_ReturnIsCorrect()
        {
            //Declaration Variables
            var project = new Project()
            {
                Id = 1,
                Name = "Project one",
                Description = "",
                CompanyId = 1
            };
            var user = new User()
            {
                Id = "1"
            };

            //Create simulated service
            var projectService = ServiceUtilities.CreateProjectServices(
                out _,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out Mock<IAsyncRepository<Project>> projectRepository,
                out _,
                out _,
                out _,
                out _, out _);

            //configuration Test

            projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
                It.IsAny<int>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync((Project)null);

            //validated test

            var result = await projectService.DeleteProject(user.Id, project.Id);
            //Assert.True(result.IsSuccess);
            //Assert.False(result.IsSuccess);
            //Assert.Equal(ErrorStrings.CompanyUserNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.ProjectNotFound);
        }

        /// <summary>
        /// check result when User not belong to the company
        /// </summary>
        /// <returns>Error, User not belong to the company</returns>
        [Fact]
        public async Task WhenDeleteProjectAsyncUserDoNotBelongToTheCompany_ReturnIsCorrect()
        {
            //Declaration Variables
            var project = new Project()
            {
                Id = 1,
                Name = "Project one",
                Description = "",
                CompanyId = 1
            };
            var user = new User()
            {
                Id = "1"
            };
            var companyUser = new CompanyUser()
            {
                Id = 1,
                UserName = "name",
                UserId = "1"
            };


            //Create simulated service
            var projectService = ServiceUtilities.CreateProjectServices(
                out _,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out Mock<IAsyncRepository<Project>> projectRepository,
                out _,
                out _,
                out _,
                out _, out _);

            //configuration Test

            projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
                It.IsAny<int>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(project);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>()
                    )).ReturnsAsync(false);

            //validated test
            var result = await projectService.DeleteProject(user.Id, project.Id);
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserDoesNotBelongToTheCompany);

        }
    }
}
