using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agropop.Database.Models
{
    public class Produto
    {
        [Key]
        public Guid ProdutoId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(255)]
        public string Nome { get; set; }

        [Required]
        public decimal Valor { get; set; }

        [Required]
        public decimal PesoCheio { get; set; }

        [Required]
        public decimal PesoVazio { get; set; }

        [Required]
        public decimal VolumeEmbalagem { get; set; }

        public ICollection<Estoque> Estoques { get; } = new List<Estoque>();

    }
}
