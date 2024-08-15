namespace NetForemost.Core.Dtos.AppClients
{
    public class AppClientDto
    {
        public string AppName { get; set; }
        public string ClientName { get; set; }
        public Guid ApiKey { get; set; }
        public string Description { get; set; }
    }
}
