using Ardalis.Specification;
using NetForemost.Core.Entities.StoryPoints;

namespace NetForemost.Core.Specifications.StoryPoints
{
    public class FindAllStoryPointsSpecification : Specification<StoryPoint>
    {
        public FindAllStoryPointsSpecification()
        {
            Query.OrderBy(sp => sp.Points);
        }
    }
}
