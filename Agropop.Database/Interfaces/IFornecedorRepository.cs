using Agropop.Database.Interfaces;
using Agropop.Database.Models;
using System;
using System.Collections.Generic;

namespace Agropop.Database.Repository
{
    public interface IFabricanteRepository : IRepository<Fabricante>
    {
        IEnumerable<Fabricante> FindFabricantes(Func<Fabricante, bool> predicate);
    }
}