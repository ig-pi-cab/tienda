using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{//Definicion de plantilla o contrato.
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);//Obtiene entidad por idenfiticador
        Task<IEnumerable<T>> GetAllAsync();//Obtiene todos los recursos
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);//Regresa un conjunto de registros dependiendo de la expresion
        Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(int pageIndex, int pageSize, string search);
        void Add(T entity);//Agrega un elemento al contexto, y el resto se explica solo.
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);

    }
}
