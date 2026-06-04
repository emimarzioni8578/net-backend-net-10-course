namespace MiPrimerApi.Models
{
    public class UserRequest
    {
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public int? CompanyId { get; set; }
    }
}
