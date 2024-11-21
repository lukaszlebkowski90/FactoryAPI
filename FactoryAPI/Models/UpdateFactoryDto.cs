using System.ComponentModel.DataAnnotations;

namespace FactoryAPI.Models
{
    public class UpdateFactoryDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactEmail { get; set; }
    }
}
