using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Tasks
{
    public class PutTaskTypeRequest
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}
