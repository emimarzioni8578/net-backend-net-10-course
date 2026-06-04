namespace MiPrimerApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public virtual Address? Address { get; set; }
        public int? CompanyId { get; set; } = 0;
        public virtual Company? Company { get; set; }
    }
}
