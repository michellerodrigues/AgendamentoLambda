using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstoqueService.Data.Models;

namespace EstoqueService.Data.Interfaces
{
    public interface IRevendedorRepository : IRepository<Revendedor>
    {
        IEnumerable<Revendedor> FindRevendedor(Func<Revendedor, bool> predicate);
    }
}