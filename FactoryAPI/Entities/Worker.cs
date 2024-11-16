namespace FactoryAPI.Entities
{
    public class Worker
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Salary { get; set; }

        public int JobSeniority { get; set; }

        public int FactoryId { get; set; }
        public virtual Factory Factory { get; set; }
    }
}
