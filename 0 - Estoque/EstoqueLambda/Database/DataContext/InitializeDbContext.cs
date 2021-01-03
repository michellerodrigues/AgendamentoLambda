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
                var Fabricante1 = new Fabricante(){Email = "mica-fabricante1@mailinator.com",Id = Guid.NewGuid(),Nome = "Mica Fabricante I"};
                var Fabricante2 = new Fabricante() { Email = "mica-fabricante2@mailinator.com", Id = Guid.NewGuid(), Nome = "Mica Fabricante II" };
                var Fabricante3 = new Fabricante() { Email = "mica-fabricante3@mailinator.com", Id = Guid.NewGuid(), Nome = "Mica Fabricante III" };
                var Fabricante4 = new Fabricante() { Email = "mica-fabricante4@mailinator.com", Id = Guid.NewGuid(), Nome = "Mica Fabricante IV" };
                var Fabricante5 = new Fabricante() { Email = "mica-fabricante5@mailinator.com", Id = Guid.NewGuid(), Nome = "Mica Fabricante V" };

                var Revendedor1 = new Revendedor() { Email = "mica-revendedor1@mailinator.com", Nome = "Mica Revendedor I" };
                var Revendedor2 = new Revendedor() { Email = "mica-revendedor2@mailinator.com", Nome = "Mica Revendedor II" };
                var Revendedor3 = new Revendedor() { Email = "mica-revendedor3@mailinator.com", Nome = "Mica Revendedor III" };
                var Revendedor4 = new Revendedor() { Email = "mica-revendedor4@mailinator.com", Nome = "Mica Revendedor IV" };
                var Revendedor5 = new Revendedor() { Email = "mica-revendedor5@mailinator.com", Nome = "Mica Revendedor V" };
                var Revendedor6 = new Revendedor() { Email = "mica-revendedor6@mailinator.com", Nome = "Mica Revendedor VI" };


                var loteEstou1 = new Estoque() { DataVecimentoProduto = DateTime.Now.AddDays(-1), Descartado = false, Fabricante = Fabricante1, QtdeDispUnidade = 10, Revendedor = Revendedor1 };
                var loteEstou2 = new Estoque() { DataVecimentoProduto = DateTime.Now.AddDays(-1), Descartado = false, Fabricante = Fabricante1, QtdeDispUnidade = 10, Revendedor = Revendedor2 };
                var loteEstou3 = new Estoque() { DataVecimentoProduto = DateTime.Now.AddDays(-1), Descartado = false, Fabricante = Fabricante1, QtdeDispUnidade = 10, Revendedor = Revendedor2 };
                var loteEstou4 = new Estoque() { DataVecimentoProduto = DateTime.Now.AddDays(-1), Descartado = false, Fabricante = Fabricante1, QtdeDispUnidade = 10, Revendedor = Revendedor3 };

                var loteEstou5 = new Estoque() { DataVecimentoProduto = DateTime.Now.AddDays(-1), Descartado = false, Fabricante = Fabricante2, QtdeDispUnidade = 10, Revendedor = Revendedor1 };
                var loteEstou6 = new Estoque() { DataVecimentoProduto = DateTime.Now.AddDays(-1), Descartado = false, Fabricante = Fabricante2, QtdeDispUnidade = 10, Revendedor = Revendedor2 };
                var loteEstou7 = new Estoque() { DataVecimentoProduto = DateTime.Now.AddDays(-1), Descartado = false, Fabricante = Fabricante2, QtdeDispUnidade = 10, Revendedor = Revendedor5 };
                var loteEstou8 = new Estoque() { DataVecimentoProduto = DateTime.Now.AddDays(-1), Descartado = false, Fabricante = Fabricante2, QtdeDispUnidade = 10, Revendedor = Revendedor4 };


                var loteEstou9 = new Estoque() { DataVecimentoProduto = DateTime.Now.AddDays(-1), Descartado = false, Fabricante = Fabricante3, QtdeDispUnidade = 10, Revendedor = Revendedor1 };
                var loteEstou10 = new Estoque() { DataVecimentoProduto = DateTime.Now.AddDays(-1), Descartado = false, Fabricante = Fabricante4, QtdeDispUnidade = 10, Revendedor = Revendedor1 };


                var loteEstou11 = new Estoque() { DataVecimentoProduto = DateTime.Now.AddDays(-1), Descartado = false, Fabricante = Fabricante3, QtdeDispUnidade = 11, Revendedor = Revendedor6 };


                var loteEstou12 = new Estoque() { DataVecimentoProduto = DateTime.Now.AddDays(-1), Descartado = false, Fabricante = Fabricante5, QtdeDispUnidade = 12, Revendedor = Revendedor1 };


                var produto1 = new Produto { Nome = "Cupinicida 3MAX ", Valor = 5662, Id = Guid.NewGuid(), PesoCheio = 20, PesoVazio = 3, VolumeEmbalagem = 3L };
                var produto2 = new Produto { Nome = "Herbicida 3MAX ", Valor = 5021, Id = Guid.NewGuid(), PesoCheio = 10, PesoVazio = 3, VolumeEmbalagem = 1L };
                var produto3 = new Produto { Nome = "Pesticida 3MAX ", Valor = 9652, Id = Guid.NewGuid(), PesoCheio = 20, PesoVazio = 3, VolumeEmbalagem = 1L };
                var produto4 = new Produto { Nome = "Protol 3MAX ", Valor = 321, Id = Guid.NewGuid(), PesoCheio = 20, PesoVazio = 3, VolumeEmbalagem = 5L };
                var produto5 = new Produto { Nome = "Protol 4MAX ", Valor = 321, Id = Guid.NewGuid(), PesoCheio = 15, PesoVazio = 3, VolumeEmbalagem = 1L };
                var produto6 = new Produto { Nome = "Protol 5MAX ", Valor = 321, Id = Guid.NewGuid(), PesoCheio = 5, PesoVazio = 3, VolumeEmbalagem = 2L };

                produto1.Estoques.Add(loteEstou1);
                produto1.Estoques.Add(loteEstou2);
                produto1.Estoques.Add(loteEstou3);

                produto2.Estoques.Add(loteEstou4);
                produto2.Estoques.Add(loteEstou4);
                produto2.Estoques.Add(loteEstou5);

                produto3.Estoques.Add(loteEstou6);
                produto3.Estoques.Add(loteEstou9);
                produto3.Estoques.Add(loteEstou10);


                produto4.Estoques.Add(loteEstou6);
                produto4.Estoques.Add(loteEstou9);
                produto4.Estoques.Add(loteEstou10);

                produto5.Estoques.Add(loteEstou11);
                produto5.Estoques.Add(loteEstou12);

                produto6.Estoques.Add(loteEstou7);
                produto6.Estoques.Add(loteEstou8);

                context.Produtos.Add(produto1);
                context.Produtos.Add(produto2);
                context.Produtos.Add(produto3);
                context.Produtos.Add(produto4);
                context.Produtos.Add(produto5);
                context.Produtos.Add(produto6);
                
                context.SaveChanges();
            }
        }
    }
}
