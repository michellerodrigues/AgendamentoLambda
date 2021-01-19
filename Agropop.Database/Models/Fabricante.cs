using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agropop.Database.Models
{
    public class Fabricante
    {
        [Key]
        public Guid FabricanteId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(255)]
        public string Nome { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }
     
        public ICollection<Estoque> Estoques { get; } = new List<Estoque>();
    }
}
