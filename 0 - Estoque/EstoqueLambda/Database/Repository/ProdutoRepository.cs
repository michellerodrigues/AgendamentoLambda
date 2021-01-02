using EstoqueLambda.Database.Interfaces;
using EstoqueLambda.Database.Models;
using EstoqueLambda.Database.Repository;
using EstoqueLambda.Database.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstoqueLambda.Database.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(DescarteDataContext context) : base(context)
        {
        }
        public IEnumerable<Produto> FindProdutosVencidos()
        {
            return _context.Produtos.Where(pd => pd.DataVencimento <= DateTime.Now);
        }
    }

}
