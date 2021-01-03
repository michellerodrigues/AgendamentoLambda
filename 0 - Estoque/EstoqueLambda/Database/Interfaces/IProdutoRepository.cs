using EstoqueLambda.Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstoqueLambda.Database.Interfaces
{
    public interface IProdutoRepository
    {
        IEnumerable<Produto> FindProdutoByName(string name);
    }
}