using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Company_Project.Model
{
    public class Companies
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Describtion { get; set; }
        public byte[]? Image { get; set; }

        public bool IsAgent { get; set; }
        [NotMapped]
        public string Imagesrc
        {
            get
            {
                if (Image == null)
                {
                    return string.Empty;
                }

                try
                {
                    if (!ValidateImageFormat(Image))
                    {
                        throw new ArgumentException("Invalid image format. Supported formats: JPG, JPEG, PNG");
                    }

                    string base64String = Convert.ToBase64String(Image);
                    string mimeType = GetImageMimeType(Image);

                    return $"data:{mimeType};base64,{base64String}";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting image to base64: {ex.Message}");
                    return string.Empty;
                }
            }
        }

        private bool ValidateImageFormat(byte[] imageData)
        {
            if (imageData.Length < 3)
            {
                return false;
            }

            return (imageData[0] == 0xFF && imageData[1] == 0xD8 && imageData[2] == 0xFF)
                   || (imageData[0] == 0x89 && imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47);
        }

        private string GetImageMimeType(byte[] imageData)
        {
            if (imageData.Length < 3)
            {
                return "application/octet-stream";
            }

            return (imageData[0] == 0xFF && imageData[1] == 0xD8 && imageData[2] == 0xFF) ? "image/jpeg"
                   : (imageData[0] == 0x89 && imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47) ? "image/png"
                   : "application/octet-stream";
        }
        [NotMapped]
        [JsonIgnore]
        public virtual List<Departments>? Departments { get; set; }
    }
}
