using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Goals;
public class GoalExtraMile : BaseEntity
{
    public DateTime ExtraMileTargetEndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public int GoalId { get; set; }
    public virtual Goal Goal { get; set; }
    public bool IsVoided { get; set; }
    public bool IsActive { get; set; }
    public int? GoalStatusId { get; set; }
    public virtual GoalStatus? GoalStatus { get; set; }

    public void CancellExtraMile(string userId)
    {
        // The extra mile associated with that goal is disabled
        IsActive = false;
        ActualEndDate = null;
        GoalStatusId = null;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = userId;
    }
}
