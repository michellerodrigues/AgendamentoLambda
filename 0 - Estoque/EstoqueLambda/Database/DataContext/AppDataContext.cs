using EstoqueService.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace EstoqueService.DataContext{
    
    public class AppDataContext : DbContext{

        public AppDataContext(DbContextOptions<AppDataContext> options) : base (options)
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