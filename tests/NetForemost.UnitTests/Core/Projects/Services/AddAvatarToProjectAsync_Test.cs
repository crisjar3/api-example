using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
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
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using NetForemost.Core.Entities.CDN;

namespace NetForemost.UnitTests.Core.Projects.Services
{
    public class AddAvatarToProjectAsync_Test
    {
        /// <summary>
        /// check all is correct when add a Project avatar to a project
        /// </summary>
        /// <returns>result.IsSuccess</returns>
        [Fact]
        public async Task WhenAllIsCorrect_ReturnCorrect()
        {
            //Declaration Variable
            var name = "Name";
            var description = "Description";
            var project = new Project() { Id = 1 };
            var projectAvatar = new ProjectAvatar() { Id = 1, AvatarImageUrl = "Image" };
            User user = new();
            var image = new List<IFormFile>();
            image.Add(new FormFile(
            baseStream: new MemoryStream(10),
            baseStreamOffset: 0,
            length: 10,
            name: "Image",
            fileName: "image.png"
            )
            {
                Headers = new HeaderDictionary(),
                ContentType = "Content;Content"
            });

            //Create Simulated Service
            var projectService = ServiceUtilities.CreateProjectServices(
                 out _,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out Mock<IAsyncRepository<Project>> projectRepository,
                out _,
                out _,
                out Mock<IAsyncRepository<ProjectAvatar>> projectAvatarRepository,
                out Mock<StorageClient> storageClient,
                out Mock<IOptions<CloudStorageConfig>> config);

            //Configuration for test
            projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(project);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(true);


            //Validation Test
            var result = await projectService.AddAvatarToProjectAsync(name, description, project.Id, image, user.Id);

            Assert.True(result.IsSuccess);
        }

        /// <summary>
        /// check when Project not Found
        /// </summary>
        /// <returns>ErrorStrings.ProjectNotFound</returns>
        [Fact]
        public async Task WhenProjectNotFound_ReturnCorrect()
        {
            //Declaration Variable
            var name = "Name";
            var description = "Description";
            var project = new Project() { Id = 1 };
            var projectAvatar = new ProjectAvatar() { Id = 1, AvatarImageUrl = "Image" };
            User user = new();
            var image = new List<IFormFile>();
            image.Add(new FormFile(
            baseStream: new MemoryStream(10),
            baseStreamOffset: 0,
            length: 10,
            name: "Image",
            fileName: "image.png"
            )
            {
                Headers = new HeaderDictionary(),
                ContentType = "Content;Content"
            });

            //Create Simulated Service
            var projectService = ServiceUtilities.CreateProjectServices(
                 out _,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out _,
            out Mock<IAsyncRepository<ProjectAvatar>> projectAvatarRepository,
            out Mock < StorageClient > storageClient,
            out Mock<IOptions<CloudStorageConfig>> config);

            //Configuration for test
            projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())).ReturnsAsync((Project)null);

            //Validation Test
            var result = await projectService.AddAvatarToProjectAsync(name, description, project.Id, image, user.Id);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.ProjectNotFound.Replace("[id]", project.Id.ToString()));
        }

        /// <summary>
        /// check all is correct when add a User do not belong to the Company
        /// </summary>
        /// <returns>ErrorStrings.UserDoesNotBelongToTheCompany</returns>
        [Fact]
        public async Task WhenUserDoesNotBelongToTheCompany_ReturnCorrect()
        {
            //Declaration Variable
            var name = "Name";
            var description = "Description";
            var project = new Project() { Id = 1 };
            var projectAvatar = new ProjectAvatar() { Id = 1, AvatarImageUrl = "Image" };
            User user = new();
            var image = new List<IFormFile>();
            image.Add(new FormFile(
            baseStream: new MemoryStream(10),
            baseStreamOffset: 0,
            length: 10,
            name: "Image",
            fileName: "image.png"
            )
            {
                Headers = new HeaderDictionary(),
                ContentType = "Content;Content"
            });

            //Create Simulated Service
            var projectService = ServiceUtilities.CreateProjectServices(
                 out _,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out Mock<IAsyncRepository<Project>> projectRepository,
                out _,
                out _,
                out Mock<IAsyncRepository<ProjectAvatar>> projectAvatarRepository,
                out Mock<StorageClient> storageClient,
                out Mock<IOptions<CloudStorageConfig>> config);

            //Configuration for test
            projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(project);

            companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
                It.IsAny<CheckUserIsInCompanySpecification>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(false);

            //Validation Test
            var result = await projectService.AddAvatarToProjectAsync(name, description, project.Id, image, user.Id);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserDoesNotBelongToTheCompany);
        }
    }
}
