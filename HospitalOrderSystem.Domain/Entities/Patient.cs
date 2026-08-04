using HospitalOrderSystem.Domain.Enums;
using System;

namespace HospitalOrderSystem.Domain.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string TcNo { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        private DateTime dateCheck;
        public DateTime BirthDate
        {
            get => dateCheck;
            set
            {
                if (value.Date > DateTime.UtcNow.Date)
                    throw new ArgumentException("Doğum tarihi gelecekte olamaz.", nameof(BirthDate));

                dateCheck = value;
            }
        }
        public Gender Gender { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public InsuranceType? InsuranceType { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
