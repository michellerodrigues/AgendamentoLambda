using EstoqueService.DataContext;
using EstoqueService.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EstoqueService.Data
{
    public static class DBInicializar
    {
        public static void StartDataBase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDataContext>();
                if(context.Database.EnsureCreated())
                {
                    var revendedor1 = new Revendedor { Nome="Michelle Rodrigues 1 ",Email="mica-revenda1@mailinator.com"};
                    var revendedor2 = new Revendedor { Nome="Michelle Rodrigues 2",Email="mica-revenda2@mailinator.com"};
                    var revendedor3 = new Revendedor { Nome="Michelle Rodrigues 3",Email="mica-revenda3@mailinator.com"};
                    
                    
                    context.Revendedores.Add(revendedor1);
                    context.Revendedores.Add(revendedor2);
                    context.Revendedores.Add(revendedor3);


                    var produto1 = new Produto { Nome="Cupinicida 3MAX ", Valor=835, PesoCheio=450, PesoVazio=10,VolumeEmbalagem=0.3M}; 
                    var produto2 = new Produto { Nome="Herbicidas Hortalis", Valor=635, PesoCheio=450, PesoVazio=10,VolumeEmbalagem=0.3M}; 
                    var produto3 = new Produto { Nome="Inseticida Fatalis", Valor=395, PesoCheio=450, PesoVazio=10,VolumeEmbalagem=0.3M}; 

                    var fabricante1 = new Fabricante { Nome="Fabricante 1 ",Email="mica-Fabricante1@mailinator.com"};
                    
                    var fabricante2 = new Fabricante { Nome="Fabricante 2",Email="mica-Fabricante2@mailinator.com"};
                    
                    var fabricante3 = new Fabricante { Nome="Fabricante 3",Email="mica-Fabricante3@mailinator.com"};

                    context.Fabricantes.Add(fabricante1);
                    context.Fabricantes.Add(fabricante2);
                    context.Fabricantes.Add(fabricante3);

                    context.Produtos.Add(produto1);
                    context.Produtos.Add(produto2);
                    context.Produtos.Add(produto3);
                                       
                    //context.SaveChanges();

                    var estoque = new Estoque{DataInclusao=DateTime.Now,DataVecimentoProduto=DateTime.Now.AddDays(-15),Descartado=false, Fabricante = fabricante3,Revendedor=revendedor1, Produto = produto1, QtdeDispUnidade=20,Lote="001",Serie="aab"};
                    var estoque1 = new Estoque{DataInclusao=DateTime.Now.AddHours(-1),DataVecimentoProduto=DateTime.Now.AddDays(-15),Descartado=false, Fabricante = fabricante3,Revendedor=revendedor3,Produto = produto1,QtdeDispUnidade=20,Lote="001",Serie="axb"};
                    var estoque2 = new Estoque{DataInclusao=DateTime.Now.AddHours(-2),DataVecimentoProduto=DateTime.Now.AddDays(-45),Descartado=false, Fabricante = fabricante1,Revendedor=revendedor1,Produto = produto2,QtdeDispUnidade=0,Lote="001",Serie="aab"};
                    var estoque3 = new Estoque{DataInclusao=DateTime.Now.AddHours(-3),DataVecimentoProduto=DateTime.Now.AddDays(-5),Descartado=false, Fabricante = fabricante1,Revendedor=revendedor2,Produto = produto2,QtdeDispUnidade=65,Lote="001",Serie="aab"};
                    var estoque4 = new Estoque{DataInclusao=DateTime.Now.AddHours(-4),DataVecimentoProduto=DateTime.Now.AddDays(-13),Descartado=false, Fabricante = fabricante3,Revendedor=revendedor3,Produto = produto3,QtdeDispUnidade=67,Lote="003",Serie="aax"};
                    var estoque5 = new Estoque{DataInclusao=DateTime.Now.AddHours(-5),DataVecimentoProduto=DateTime.Now.AddDays(-22),Descartado=false, Fabricante = fabricante3,Revendedor=revendedor2,Produto = produto3,QtdeDispUnidade=56,Lote="002",Serie="aac"};
                    
                    context.Estoques.Add(estoque);
                    context.Estoques.Add(estoque1);
                    context.Estoques.Add(estoque2);
                    context.Estoques.Add(estoque3);
                    context.Estoques.Add(estoque4);
                    context.Estoques.Add(estoque5);

                    context.SaveChanges();
                }
                
            }
        }
    }
}