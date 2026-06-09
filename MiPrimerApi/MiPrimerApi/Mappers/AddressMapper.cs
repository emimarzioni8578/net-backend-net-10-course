using MiPrimerApi.Entities;
using MiPrimerApi.Models;

namespace MiPrimerApi.Mappers
{
    public static class AddressMapper
    {
        public static Address ToEntity(this AddressRequest request)
        {
            return new Address
            {
                Street = request.Street,
                Suite = request.Suite,
                City = request.City,
                Zipcode = request.Zipcode,
                UserId = request.UserId
            };
        }
        public static AddressResponse ToDto(this Address entity)
        {
            return new AddressResponse
            {
                Id = entity.Id,
                Street = entity.Street,
                Suite = entity.Suite,
                City = entity.City,
                Zipcode = entity.Zipcode,
                UserId = entity.UserId,
                User = entity.User?.ToDto()
            };
        }
    }
}
