using System.Text.Json.Serialization;

namespace Company_Project.DTO
{
    public class DepartmentsWithoutSrcDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public byte[]? Image { get; set; }
        public string? Describtion { get; set; }

        [JsonIgnore]
        public IFormFile? client_file { get; set; }
        public int Company_no { get; set; }
    }
}
