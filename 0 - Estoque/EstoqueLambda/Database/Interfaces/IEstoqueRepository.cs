using EstoqueLambda.Database.Models;
using System.Collections.Generic;

namespace EstoqueLambda.Database.Interfaces
{
    public interface IEstoqueRepository : IRepository<Estoque>
    {
        IEnumerable<Estoque> FindItensVencidosEstoque();

        IEnumerable<Estoque> FindItensFinalizadosEstoque();
    }

}