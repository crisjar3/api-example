namespace NetForemost.SharedKernel.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public bool isDeleted { get; set; } = false;
    public DateTime? ArchivedAt { get; set; }
    public string? ArchivedBy { get; set; }
    public bool IsArchived { get; set; } = false;

    public void AddCreatedInfo(string userId)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = userId;
    }

    public void AddUpdateInfo(string userId)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = userId;
    }

    public void AddDeleteInfo(string userId)
    {
        DeletedAt = DateTime.UtcNow;
        DeletedBy = userId;
        isDeleted = true;
    }

    public void AddArchiveInfo(string userId)
    {
        ArchivedAt = DateTime.UtcNow;
        ArchivedBy = userId;
        IsArchived = true;
    }

    public void RemoveArchivedInfo()
    {
        ArchivedAt = null;
        ArchivedBy = null;
        IsArchived = false;
    }
}