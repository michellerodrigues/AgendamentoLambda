using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EstoqueLambda.Database.Models
{
    public class Revendedor
    {
        [Key]
        public Guid RevendedorId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(255)]
        public string Nome { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }
     
        public ICollection<Estoque> Estoques { get; set;} = new List<Estoque>();

    }
}
