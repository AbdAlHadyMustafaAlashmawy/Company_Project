using System.Text.Json.Serialization;

namespace Company_Project.DTO
{
    public class ProductsWithoutSrcDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public int Department_no { get; set; }
    }
}
