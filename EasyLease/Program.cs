using System;
using System.Linq;
using EasyLease.Data;
using EasyLease.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyLease
{
    class Program
    {
        static void Main()
        {
            using var db = new AppDbContext();
            bool v = db.Database.EnsureCreated();
            SeedData(db);

            int choice;
            do
            {
                Console.Clear();
                Console.WriteLine("=== EasyLease Property Management ===");
                Console.WriteLine("1. View Properties and Units");
                Console.WriteLine("2. View Tenants");
                Console.WriteLine("3. Create Lease");
                Console.WriteLine("4. View Active Leases");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                choice = int.TryParse(Console.ReadLine(), out var num) ? num : -1;
                Console.Clear();

                switch (choice)
                {
                    case 1: ViewProperties(db); break;
                    case 2: ViewTenants(db); break;
                    case 3: CreateLease(db); break;
                    case 4: ViewLeases(db); break;
                }

                if (choice != 0)
                {
                    Console.WriteLine("\nPress any key to return to the main menu...");
                    Console.ReadKey();
                }

            } while (choice != 0);
        }

        static void SeedData(AppDbContext db)
        {
            if (!db.Properties.Any())
            {
                var prop = new Property { Name = "Maple Apartments", Address = "123 Maple St" };
                prop.Units.Add(new Unit { UnitNumber = "A1", MonthlyRent = 1200 });
                prop.Units.Add(new Unit { UnitNumber = "A2", MonthlyRent = 1300 });

                var house = new Property { Name = "Oak House", Address = "45 Oak Ave" };
                house.Units.Add(new Unit { UnitNumber = "Main", MonthlyRent = 1800 });

                db.Properties.AddRange(prop, house);
                db.Tenants.AddRange(
                    new Tenant { Name = "Alice Tenant", Email = "alice@example.com" },
                    new Tenant { Name = "Bob Renter", Email = "bob@example.com" }
                );

                db.SaveChanges();
            }
        }

        static void ViewProperties(AppDbContext db)
        {
            Console.WriteLine("=== Properties and Units ===");
            var properties = db.Properties.Include(p => p.Units);
            foreach (var p in properties)
            {
                Console.WriteLine($"\nProperty: {p.Name} ({p.Address})");
                foreach (var u in p.Units)
                    Console.WriteLine($"  Unit: {u.UnitNumber}, Rent: ${u.MonthlyRent}");
            }
        }

        static void ViewTenants(AppDbContext db)
        {
            Console.WriteLine("=== Tenants ===");
            foreach (var t in db.Tenants)
                Console.WriteLine($"{t.Id}: {t.Name} ({t.Email})");
        }

        static void CreateLease(AppDbContext db)
        {
            Console.WriteLine("=== Create New Lease ===");
            ViewTenants(db);
            Console.Write("Enter Tenant ID: ");
            int tenantId = int.Parse(Console.ReadLine() ?? "0");

            Console.WriteLine("\nAvailable Units:");
            var availableUnits = db.Units.Include(u => u.Property)
                .Where(u => !db.Leases.Any(l => l.UnitId == u.Id && l.EndDate == null));
            foreach (var u in availableUnits)
                Console.WriteLine($"{u.Id}: {u.Property!.Name} - Unit {u.UnitNumber} (${u.MonthlyRent})");

            Console.Write("Enter Unit ID: ");
            int unitId = int.Parse(Console.ReadLine() ?? "0");

            var lease = new Lease
            {
                TenantId = tenantId,
                UnitId = unitId,
                StartDate = DateTime.Now
            };

            db.Leases.Add(lease);
            db.SaveChanges();
            Console.WriteLine("Lease created successfully!");
        }

        static void ViewLeases(AppDbContext db)
        {
            Console.WriteLine("=== Active Leases ===");
            var leases = db.Leases
                .Include(l => l.Tenant)
                .Include(l => l.Unit)
                .ThenInclude(u => u.Property);
            foreach (var l in leases)
            {
                string status = l.EndDate == null ? "Active" : "Ended";
                Console.WriteLine($"{l.Id}: {l.Tenant?.Name} leasing {l.Unit?.Property?.Name} - Unit {l.Unit?.UnitNumber} ({status})");
            }
        }
    }
}
