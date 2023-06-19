using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ApiMVC.Models
{
    public partial class CallDbContext : DbContext
    {
        public CallDbContext()
            : base("name=CallDbContext")
        {

        }

        public DbSet<Call> Calls { get; set; }  
        public DbSet<CallHistory> CallsHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Call>();
            modelBuilder.Entity<CallHistory>();
        }
    }
}
