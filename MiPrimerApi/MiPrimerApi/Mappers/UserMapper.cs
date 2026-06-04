using MiPrimerApi.Entities;
using MiPrimerApi.Models;

namespace MiPrimerApi.Mappers
{
    public static class UserMapper
    {
        public static UserResponse ToDto(this User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Phone = user.Phone,
                Website = user.Website
            };
        }
        public static User ToEntity(this UserRequest request)
        {
            return new User
            {
                Name = request.Name,
                Username = request.Username,
                Phone = request.Phone,
                Website = request.Website
            };
        }

        public static User ToEntity(this UserRequest request, int id)
        {
            return new User
            {
                Id = id,
                Name = request.Name,
                Username = request.Username,
                Phone = request.Phone,
                Website = request.Website
            };
        }

        public static IEnumerable<UserResponse> ToDtoList(this IEnumerable<User> users)
        {
            return users.Select(x => ToDto(x)).ToList();
        }
    }
}
