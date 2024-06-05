using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Company_Project.DTO
{
    public class CompaniesDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public IFormFile? client_file { get; set; }
        public string? Describtion { get; set; }
        public string? Imagesrc { get; set; }
        [JsonIgnore]
        public byte[]? Image { get; set; }
        public bool IsAgent { get; set; }
    }
}
