using NetForemost.Core.Specifications.Tasks;
using NetForemost.SharedKernel.Interfaces;
using TaskEntity = NetForemost.Core.Entities.Tasks.Task;


namespace NetForemost.Core.Extensions;
public static class RepositoryExtensions
{
    public static async Task<bool> ValidateTasksBelongToUsers(this IAsyncRepository<TaskEntity> repo, string userId, params int[] ids)
    {

        foreach (var id in ids)
        {
            var result = await repo.FirstOrDefaultAsync(new GetTaskByUserId(userId, id));

            if (result is null)
            {
                return false;
            }
        }
        return true;
    }
}
