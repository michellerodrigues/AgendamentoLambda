using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EstoqueService.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        void Create(T entity);
        void Remove(T entity);
    }    
}