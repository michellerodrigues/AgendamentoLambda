using Agropop.Database.Models;
using System;
using System.Collections.Generic;

namespace Agropop.Database.Interfaces
{
    public interface IRevendedorRepository : IRepository<Revendedor>
    {
        IEnumerable<Revendedor> FindRevendedor(Func<Revendedor, bool> predicate);
    }
}