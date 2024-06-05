using System.Text.Json.Serialization;

namespace Company_Project.DTO
{
    public class CompaniesWithoutIImageFile
    {
        public int id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public string? Describtion { get; set; }
        public string? Imagesrc { get; set; }
        [JsonIgnore]
        public byte[]? Image { get; set; }
        public bool IsAgent { get; set; }
    }
}
