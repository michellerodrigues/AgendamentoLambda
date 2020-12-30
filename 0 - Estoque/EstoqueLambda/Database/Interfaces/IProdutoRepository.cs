using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstoqueService.Data.Models;

namespace EstoqueService.Data.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> FindProdutos(Func<Produto, bool> predicate);       
    }
}