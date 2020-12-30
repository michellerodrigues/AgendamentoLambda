using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EstoqueService.Data.JoinFacade;

namespace EstoqueService.Data.Models
{
    public class Produto
    {
        [Key]
        public Guid Id { get; set; }

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

        public virtual ICollection<Estoque> Estoques { get; } = new List<Estoque>();

    }
}
