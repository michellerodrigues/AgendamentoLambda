using System;
using System.Collections.Generic;
using System.Linq;
using Agropop.Database.Models;
using Agropop.Database.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Agropop.Database.Repository
{
    public class EstoqueRepository : Repository<Estoque>, IEstoqueRepository
    {

        private new DataContext.DescarteDataContext _context;
        public EstoqueRepository(DataContext.DescarteDataContext context) : base(context)
        {
            _context = context;
            _context.Estoques.Include(_ => _.Produto).Include(e => e.Revendedor).Include(e => e.Fabricante);
        }


        public IEnumerable<Estoque> FindItensFinalizadosEstoque()
        {
            return _context.Estoques.Where(e => e.Descartado == false && e.QtdeDispUnidade <= 0)
                .Include(c => c.Fabricante).Include(e => e.Revendedor).Include(e => e.Produto)
                .OrderBy(e => e.Revendedor).ToList();
        }

        public IEnumerable<Estoque> FindItensVencidosEstoque()
        {
            DateTime dataVenc = DateTime.Now;
            return (_context.Estoques.Where(e => e.Descartado == false && e.QtdeDispUnidade > 0 && e.DataVecimentoProduto<= dataVenc)
                .Include(_ => _.Produto).Include(e => e.Revendedor).Include(e => e.Fabricante)).
                OrderBy(e => e.Fabricante).ToList();           
        }

        public IEnumerable<Estoque> AtualizarLotesEnviadosParaDescarte(Guid estoqueId)
        {
            DateTime dataVenc = DateTime.Now;
            return _context.Estoques.Where(e => e.Descartado == false && e.EstoqueId == estoqueId);
        }

    }
}
