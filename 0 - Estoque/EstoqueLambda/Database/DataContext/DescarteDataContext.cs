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

            builder.Entity<Estoque>()
              .HasOne<Produto>(s => s.Produto)
              .WithMany(g => g.Estoques)
              .HasForeignKey(s => s.ProdutoId);

            builder.Entity<Estoque>()
              .HasOne<Fabricante>(s => s.Fabricante)
              .WithMany(g => g.Estoques)
              .HasForeignKey(s => s.FabricanteId);

            builder.Entity<Estoque>()
              .HasOne<Revendedor>(s => s.Revendedor)
              .WithMany(g => g.Estoques)
              .HasForeignKey(s => s.RevendedorId);
            

            base.OnModelCreating(builder);
        }
    }
}