namespace NetForemost.Infrastructure.Importer.Entities
{
    public class JobRoleImporter
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int JobRoleCategoryId { get; set; }
        public bool IsDefault { get; set; }
    }
}
