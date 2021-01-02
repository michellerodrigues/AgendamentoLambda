using EstoqueLambda.Database.DataContext;
using EstoqueLambda.Database.Interfaces;
using EstoqueLambda.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using EstoqueLambda.Database.Models;

public class RevendedorRepository : Repository<Revendedor>, IRevendedorRepository
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