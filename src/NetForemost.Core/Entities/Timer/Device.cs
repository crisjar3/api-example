using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Timer;

public class Device : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int DeviceTypeId { get; set; }

    public virtual DeviceType DeviceType { get; set; }
}