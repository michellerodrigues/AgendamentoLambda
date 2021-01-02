using EstoqueLambda.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EstoqueLambda.Database.DataContext
{
    public class InitializeDbContext
    {
        public InitializeDbContext()
        {
            var context = new DescarteDataContextFactory().CreateDbContext(null);

            if (context.Database.EnsureCreated())
            {
                var produto1 = new Produto { Nome = "Cupinicida 3MAX ", DataVencimento = DateTime.Now.AddDays(45), Id = new Guid() };

                context.Produtos.Add(produto1);

                context.SaveChanges();
            }
        }
    }
}
