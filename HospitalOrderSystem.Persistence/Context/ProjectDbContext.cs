using HospitalOrderSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalOrderSystem.Persistence.Context;

public class ProjectDbContext : DbContext
{
    public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
    {
    }
    public DbSet<Patient> Patients { get; set; } = null!;
}
