namespace HospitalOrderSystem.Domain.Enums
{
    public enum ActionType : byte
    {
        Created = 1,
        Submitted = 2,
        Approved = 3,
        Rejected = 4,
        StatusChanged = 5,
        Completed = 6,
        Cancelled = 7,
        Edited = 8
    }
}
