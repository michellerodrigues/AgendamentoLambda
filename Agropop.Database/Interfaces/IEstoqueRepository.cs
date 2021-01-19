using Agropop.Database.Models;
using System;
using System.Collections.Generic;

namespace Agropop.Database.Interfaces
{
    public interface IEstoqueRepository : IRepository<Estoque>
    {
        IEnumerable<Estoque> FindItensVencidosEstoque();

        IEnumerable<Estoque> FindItensFinalizadosEstoque();

        IEnumerable<Estoque> AtualizarLotesEnviadosParaDescarte(Guid estoqueId);
    }

}