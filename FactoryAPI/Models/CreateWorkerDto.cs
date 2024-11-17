using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryAPI.Models
{
    public class CreateWorkerDto
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Precision(8, 2)]
        public decimal Salary { get; set; }
        [Range(0, 50)]
        public int JobSeniority { get; set; }

        public int FactoryId { get; set; }

        [NotMapped]
        public string FullName { get; set; }
    }
}
