using Entidade;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio
{
    public class AmigoContext : DbContext
    {
        public DbSet<Amigo> amigo { get; set; }
      
        public AmigoContext() { }

        public AmigoContext(DbContextOptions<AmigoContext> options)
            : base(options) {
            Database.EnsureCreated();
        }
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AmigoContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
