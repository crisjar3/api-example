using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.AppClients
{
    public class AppClient : BaseEntity
    {
        public string AppName { get; set; }
        public string ClientName { get; set; }
        public Guid ApiKey { get; set; }
        public string Description { get; set; }
    }
}
