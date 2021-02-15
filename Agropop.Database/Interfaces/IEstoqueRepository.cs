using Agropop.Database.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agropop.Database.Interfaces
{
    public interface IEstoqueRepository : IRepository<Estoque>
    {
        IEnumerable<Estoque> FindItensVencidosEstoque();

        IEnumerable<Estoque> FindItensFinalizadosEstoque();

        Task AtualizarLotesEnviadosParaDescarte(Guid estoqueId);
    }

}