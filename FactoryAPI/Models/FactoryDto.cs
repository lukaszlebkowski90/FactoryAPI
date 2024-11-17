using FactoryAPI.Entities;

namespace FactoryAPI.Models
{
    public class FactoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactEmail { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }

        public List<Worker> Workers { get; set; }
    }
}
