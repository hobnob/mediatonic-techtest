
using System.Collections.Generic;

namespace MediatonicApi.Models.Services
{
    public interface IService<T>
    {
        void Add(T entity);
        void Update(T entity);
        T FindOne(uint id);
        IEnumerable<T> FindAll();
    }
}
