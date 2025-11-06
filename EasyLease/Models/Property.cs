using System.Collections.Generic;

namespace EasyLease.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<Unit> Units { get; set; } = new();
    }
}
