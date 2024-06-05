using System.Text.Json.Serialization;

namespace Company_Project.DTO.ReceversDTOs
{
    public class CompanyPostDTO
    {
        public string Name { get; set; }
        [JsonIgnore]
        public IFormFile? client_file { get; set; }
        public string? Describtion { get; set; }
        [JsonIgnore]
        public bool IsAgent { get; set; }
    }
}
