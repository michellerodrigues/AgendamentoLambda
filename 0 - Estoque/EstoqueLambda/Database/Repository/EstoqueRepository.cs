using System;
using System.Collections.Generic;
using System.Linq;
using EstoqueService.DataContext;
using EstoqueService.Data.Interfaces;
using EstoqueService.Data.Models;
using EstoqueService.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EstoqueService.Services.Util;

public class EstoqueRepository : Repository<Estoque>, IEstoqueRepository
{

    private new AppDataContext _context;
    public EstoqueRepository(AppDataContext context) : base(context)
    {
        _context = context;
    }


    public IEnumerable<Estoque> FindItensFinalizadosEstoque()
    {
        return _context.Estoques.Where(e => e.Descartado == false && e.QtdeDispUnidade <= 0)
            .Include(c => c.Fabricante).Include(e => e.Revendedor).Include(e => e.Produto)
            .OrderBy(e => e.Revendedor).ToList();
    }

    public IEnumerable<Estoque> FindItensVencidosEstoque()
    {
        return _context.Estoques.Where(e=>e.Descartado==false && e.DataVecimentoProduto.ToOADate()<=DateTime.Now.ToOADate() && e.QtdeDispUnidade>0)
            .Include(c => c.Fabricante).Include(e=>e.Revendedor).Include(e=>e.Produto)
            .OrderBy(e=>e.Fabricante).ToList();
    }
}