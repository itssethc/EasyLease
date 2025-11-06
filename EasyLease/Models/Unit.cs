using System.Collections.Generic;

namespace EasyLease.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public string UnitNumber { get; set; } = string.Empty;
        public decimal MonthlyRent { get; set; }

        public int PropertyId { get; set; }
        public Property? Property { get; set; }
        public List<Lease> Leases { get; set; } = new();
    }
}
