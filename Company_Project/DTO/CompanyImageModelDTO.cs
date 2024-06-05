using System.ComponentModel.DataAnnotations;

namespace Company_Project.DTO
{
    public class CompanyImageModelDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public IFormFile? Image { get; set; }
        public int Department_no { get; set; }
    }
}
