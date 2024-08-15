using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Timer;

public class DeviceType : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
}