using System.Text.Json.Serialization;

namespace Company_Project.DTO.ReceversDTOs
{
    public class ProductPostDTO
    {
        public string Name { get; set; }
        public string? Details { get; set; }
        [JsonIgnore]
        public IFormFile? client_file { get; set; }

        public int Department_no { get; set; }
    }
}
