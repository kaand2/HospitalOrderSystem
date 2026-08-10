using HospitalOrderSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalOrderSystem.Persistence.Context;

public class ProjectDbContext : DbContext
{
    public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
    {
    }
    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderAction> OrderActions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(patient => patient.Id);

            entity.HasIndex(patient => patient.TcNo)
                .IsUnique();

            entity.Property(patient => patient.TcNo)
                .HasMaxLength(11)
                .IsRequired();

            entity.Property(patient => patient.FirstName)
                .IsRequired();

            entity.Property(patient => patient.LastName)
                .IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Id);

            entity.HasIndex(user => user.Username)
                .IsUnique();

            entity.Property(user => user.Username)
                .IsRequired();

            entity.Property(user => user.PasswordHash)
                .IsRequired();

            entity.Property(user => user.FirstName)
                .IsRequired();

            entity.Property(user => user.LastName)
                .IsRequired();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(order => order.Id);

            entity.HasOne(order => order.Patient)
                .WithMany(patient => patient.Orders)
                .HasForeignKey(order => order.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(order => order.CreatedByUser)
                .WithMany(user => user.CreatedOrders)
                .HasForeignKey(order => order.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<OrderAction>(entity =>
        {
            entity.HasKey(orderAction => orderAction.Id);

            entity.HasOne(orderAction => orderAction.Order)
                .WithMany(order => order.Actions)
                .HasForeignKey(orderAction => orderAction.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(orderAction => orderAction.User)
                .WithMany(user => user.Actions)
                .HasForeignKey(orderAction => orderAction.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
