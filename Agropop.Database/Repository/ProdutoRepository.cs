using System.Collections.Generic;
using System.Linq;
using Agropop.Database.Interfaces;
using Agropop.Database.Models;
using Agropop.Database.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Agropop.Database.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(DescarteDataContext context) : base(context)
        {
        }
        public IEnumerable<Produto> FindProdutoByName(string name)
        {
            return _context.Produtos.Where(pd => pd.Nome == name);
        }
    }
}
