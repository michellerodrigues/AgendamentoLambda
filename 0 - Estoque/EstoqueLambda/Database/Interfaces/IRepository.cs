using System.Collections.Generic;

namespace EstoqueLambda.Database.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        void Create(T entity);
        void Remove(T entity);
    }    
}