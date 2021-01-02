using EstoqueLambda.Database.Models;
using System;
using System.Collections.Generic;
using EstoqueLambda.Database.Interfaces;


namespace EstoqueLambda.Database.Repository
{
    public interface IFabricanteRepository : IRepository<Fabricante>
    {
        IEnumerable<Fabricante> FindFabricantes(Func<Fabricante, bool> predicate);
    }
}