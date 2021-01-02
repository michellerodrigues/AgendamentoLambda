using System;
using System.Collections.Generic;
using EstoqueLambda.Database.Models;

namespace EstoqueLambda.Database.Interfaces
{
    public interface IRevendedorRepository : IRepository<Revendedor>
    {
        IEnumerable<Revendedor> FindRevendedor(Func<Revendedor, bool> predicate);
    }
}