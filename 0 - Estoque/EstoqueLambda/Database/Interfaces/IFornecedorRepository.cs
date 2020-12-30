using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstoqueService.Data.Models;

namespace EstoqueService.Data.Interfaces
{
    public interface IFabricanteRepository : IRepository<Fabricante>
    {
        IEnumerable<Fabricante> FindFabricantes(Func<Fabricante, bool> predicate);
    }
}