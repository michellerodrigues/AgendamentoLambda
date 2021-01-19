using Agropop.Database.Models;
using System.Collections.Generic;

namespace Agropop.Database.Interfaces
{
    public interface IProdutoRepository
    {
        IEnumerable<Produto> FindProdutoByName(string name);
    }
}