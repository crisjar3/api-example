using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Cities
{
    public class GetCitiesRequest
    {
        [Required]
        public int CountryId { get; set; }
    }
}
