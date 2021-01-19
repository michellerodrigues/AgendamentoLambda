using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Agropop.Database.Repository
{
    public class FabricanteRepository : Repository<Models.Fabricante>, IFabricanteRepository
    {

        private new DataContext.DescarteDataContext _context;
        public FabricanteRepository(DataContext.DescarteDataContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Models.Fabricante> FindFabricantes(Func<Models.Fabricante, bool> predicate)
        {
            return _context.Fabricantes.Where(predicate);
        }
    }
}
