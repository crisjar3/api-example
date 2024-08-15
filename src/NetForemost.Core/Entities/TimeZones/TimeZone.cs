using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.TimeZones;

public class TimeZone : BaseEntity
{
    public double Offset { get; set; }
    public string Text { get; set; }
}