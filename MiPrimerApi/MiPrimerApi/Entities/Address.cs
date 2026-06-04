namespace MiPrimerApi.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string? Street { get; set; }
        public string? Suite { get; set; }
        public string? City { get; set; }
        public string? Zipcode { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
