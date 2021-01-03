using System;
using System.ComponentModel.DataAnnotations;

namespace EstoqueLambda.Database.Models
{
    public class Estoque
    {
        [Key]
        public Guid EstoqueId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(3)]
        public string Serie { get; set; }

        [Required]
        public DateTime DataInclusao { get; set; } = DateTime.Now;

        [Required]
        public DateTime DataVecimentoProduto { get; set; } = DateTime.Now.AddDays(10);

        [Required]
        public decimal QtdeDispUnidade { get; set; }

        [Required]
        public bool Descartado { get; set; }
        
        public Guid FabricanteId { get; set; }
        public Fabricante Fabricante { get; set;}

        public Guid RevendedorId { get; set; }
        public Revendedor Revendedor { get; set;}

        public Guid ProdutoId { get; set; }
        public Produto Produto { get; set;} 
    }
}
