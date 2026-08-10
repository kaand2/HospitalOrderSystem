using HospitalOrderSystem.Domain.Entities;

namespace HospitalOrderSystem.Application.Interfaces.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
