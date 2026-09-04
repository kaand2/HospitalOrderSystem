using HospitalOrderSystem.Domain.Enums;

namespace HospitalOrderSystem.Application.DTOs.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CurrentOrderId { get; set; }
    }
}
