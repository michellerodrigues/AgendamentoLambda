using EstoqueLambda.Database.Interfaces;
using EstoqueLambda.Database.Models;
using EstoqueLambda.Database.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EstoqueLambda.Database.Repository
{
    public class EstoqueRepository : Repository<Estoque>, IEstoqueRepository
    {

        private new DataContext.DescarteDataContext _context;
        public EstoqueRepository(DataContext.DescarteDataContext context) : base(context)
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
            return _context.Estoques.Where(e => e.Descartado == false && e.DataVecimentoProduto.ToOADate() <= DateTime.Now.ToOADate() && e.QtdeDispUnidade > 0)
                .Include(c => c.Fabricante).Include(e => e.Revendedor).Include(e => e.Produto)
                .OrderBy(e => e.Fabricante).ToList();
        }
    }
}
