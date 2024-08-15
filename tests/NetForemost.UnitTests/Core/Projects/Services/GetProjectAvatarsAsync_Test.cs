using Moq;
using NetForemost.Core.Entities.Projects;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Projects.Services
{
    public class GetProjectAvatarsAsync_Test
    {
        /// <summary>
        /// check result when get all ProjectAvatars
        /// </summary>
        /// <returns>Success</returns>
        [Fact]
        public async Task WhenAllIsCorrect_ReturnIsCorrect()
        {
            //Declaration Variables
            var listProjectAvatars = new List<ProjectAvatar>() { new ProjectAvatar() { Id = 1 ,ProjectId=1} };
            var projecId = 1;


            //Create simulated service
            var projectService = ServiceUtilities.CreateProjectServices(
                out _,
                out _,
                out _,
                out _,
                out _,
                out Mock<IAsyncRepository<ProjectAvatar>> projectAvatarRepository,
                out _, out _);

            //configuration Test

            projectAvatarRepository.Setup(projectAvatarRepository => projectAvatarRepository.ListAsync(
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(listProjectAvatars);

            //validated test

            var result = await projectService.GetProjectAvatarsAsync(projecId);
            Assert.True(result.IsSuccess);
        }
    }
}
