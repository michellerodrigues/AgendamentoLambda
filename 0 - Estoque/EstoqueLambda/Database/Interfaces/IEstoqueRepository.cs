using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstoqueService.Data.Models;

namespace EstoqueService.Data.Interfaces
{
    public interface IEstoqueRepository : IRepository<Estoque>
    {
        IEnumerable<Estoque> FindItensVencidosEstoque();

        IEnumerable<Estoque> FindItensFinalizadosEstoque();
    }

}