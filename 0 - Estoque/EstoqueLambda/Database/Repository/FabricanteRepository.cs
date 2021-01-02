using EstoqueLambda.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using EstoqueLambda.Database.Interfaces;
using EstoqueLambda.Database.DataContext;

namespace EstoqueLambda.Database.Repository
{
    public class FabricanteRepository : Repository<Fabricante>, IFabricanteRepository
    {

        private new DescarteDataContext _context;
        public FabricanteRepository(DescarteDataContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Fabricante> FindFabricantes(Func<Fabricante, bool> predicate)
        {
            return _context.Fabricantes.Where(predicate);
        }
    }
}
