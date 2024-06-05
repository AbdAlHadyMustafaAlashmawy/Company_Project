using System.Text.Json.Serialization;

namespace Company_Project.DTO.ReceversDTOs
{
    public class DepartmentPostDTO
    {
        public string Name { get; set; }
        [JsonIgnore]
        public string? Describtion { get; set; }
        [JsonIgnore]
        public IFormFile? client_file { get; set; }
        public int Company_no { get; set; }
    }
}
