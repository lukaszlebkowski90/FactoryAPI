using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryAPI.Models
{
    public class WorkerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int JobSeniority { get; set; }

        [NotMapped]
        public string FullName { get; set; }
    }
}
