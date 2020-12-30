using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EstoqueService.Data.JoinFacade
{
    public interface IJoinEntity<TEntity>
    {
        TEntity Navigation { get; set; }
    }
}

