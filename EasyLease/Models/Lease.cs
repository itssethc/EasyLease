using System;

namespace EasyLease.Models
{
    public class Lease
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
        public int TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Unit? Unit { get; set; }
        public Tenant? Tenant { get; set; }
    }
}
