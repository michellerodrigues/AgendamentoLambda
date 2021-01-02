using EstoqueLambda.Database.Models;
using Microsoft.EntityFrameworkCore;


namespace EstoqueLambda.Database.DataContext
{

    public class DescarteDataContext : DbContext{

        public DescarteDataContext(DbContextOptions<DescarteDataContext> options) : base (options)
        {
            // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public DbSet<Produto> Produtos {get;set;}

        public DbSet<Estoque> Estoques {get;set;}   
        public DbSet<Fabricante> Fabricantes {get;set;}
        public DbSet<Revendedor> Revendedores {get;set;}

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
        }
    }
}