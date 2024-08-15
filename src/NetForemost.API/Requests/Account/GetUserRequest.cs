using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Account
{
    public class GetUseRequest
    {
        [Required]
        public string Id { get; set; }
    }
}