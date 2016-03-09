using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ShoppingCart.Models.Repositories.Concrete;

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

        void SetPagingSettings(PagingSettings pagingSettings);

        void SetSortExpression(Func<IQueryable<T>, IOrderedQueryable<T>> sortExpression);

        void SetNavigationProperties(params Expression<Func<T, object>>[] navigationProperties);

        void AddNavigationProperties(params Expression<Func<T, object>>[] navigationProperties);

        void AddIgnoreProperties(params Expression<Func<T, object>>[] ignoreProperties);

        void AddIgnoreProperty(Expression<Func<T, object>> ignoreProperty);

        void AddNavigationProperty(Expression<Func<T, object>> navigationProperty);
    }
}
