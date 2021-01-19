using System;
using System.Collections.Generic;
using System.Linq;
using Agropop.Database.Models;
using Agropop.Database.DataContext;
using Agropop.Database.Repository;

public class RevendedorRepository : Repository<Revendedor>, Agropop.Database.Interfaces.IRevendedorRepository
{
    private new DescarteDataContext _context;
    public RevendedorRepository(DescarteDataContext context) : base(context)
    {
        _context = context;
    }

    public IEnumerable<Revendedor> FindRevendedor(Func<Revendedor, bool> predicate)
    {
        return _context.Revendedores.Where(predicate);
    }
}