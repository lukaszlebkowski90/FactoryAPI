using System.ComponentModel.DataAnnotations;

namespace FactoryAPI.Models
{
    public class UpdateWorkerDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
