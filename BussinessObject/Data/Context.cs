using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace BussinessObject.Data
{
    public class Context : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<MotelChain> MotelChains { get; set; }
        public DbSet<Resident> Residents { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<ElectricityCost> ElectricityCosts { get; set; }
        public DbSet<WaterCost> WaterCosts { get; set; }

        public Context()
        {

        }

        public Context(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(local);Uid=sa;Pwd=123456;Database=MotelManagement;Trusted_Connection=True;");
        }
    }
}
