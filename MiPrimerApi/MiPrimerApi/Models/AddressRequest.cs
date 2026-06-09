using MiPrimerApi.Entities;

namespace MiPrimerApi.Models
{
    public class AddressRequest
    {
        public string? Street { get; set; }
        public string? Suite { get; set; }
        public string? City { get; set; }
        public string? Zipcode { get; set; }
        public int UserId { get; set; }
        public UserRequest? User { get; set; }
    }
}
