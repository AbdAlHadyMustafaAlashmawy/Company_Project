using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Company_Project.DTO
{
    public class ProductsWithoutIdDTO
    {
        public string Name { get; set; }
        public string? Details { get; set; }
        [JsonIgnore]
        [NotMapped]
        public IFormFile? client_file { get; set; }
        [NotMapped]
        public string? Imagesrc { get; set; }

        [NotMapped]
        [JsonIgnore]
        public byte[]? Image { get; set; }
        public int Department_no { get; set; }
    }
}
