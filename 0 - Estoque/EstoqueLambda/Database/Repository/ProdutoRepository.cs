using System;
using System.Collections.Generic;
using System.Linq;
using EstoqueService.DataContext;
using EstoqueService.Data.Interfaces;
using EstoqueService.Data.Models;
using EstoqueService.Data.Repository;
using Microsoft.EntityFrameworkCore;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    private new AppDataContext _context;
    public ProdutoRepository(AppDataContext context) : base(context)
    {
        _context = context;
    }

    public IEnumerable<Produto> FindProdutos(Func<Produto, bool> predicate)
    {
        throw new NotImplementedException();
    }
}
