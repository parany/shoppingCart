using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ShoppingCart.Models.Repositories.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        IList<T> GetAll();

        IList<T> GetList(Expression<Func<T, bool>> where);

        T GetSingle(Expression<Func<T, bool>> where);

        void Add(params T[] items);

        void Delete(params T[] items);

        void Update(params T[] items);
    }
}
