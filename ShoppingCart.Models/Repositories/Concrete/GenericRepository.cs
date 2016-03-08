using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using ShoppingCart.Models.Models.Shopping;
using ShoppingCart.Models.Repositories.Interface;

namespace ShoppingCart.Models.Repositories.Concrete
{
    public class PagingSettings
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecords { get; set; }
        public int TotalNumberOfRecords { get; set; }
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : BaseObject
    {
       
        public List<Expression<Func<T, object>>> NavigationProperties;
        private List<Expression<Func<T, object>>> _ignoreProperties;
        public PagingSettings PagingSettings = null;
        public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null;

        public GenericRepository()
        {
            SetSortExpression(list => list.OrderByDescending(p => p.DateCreated));
        }

        public virtual void SetPagingSettings(PagingSettings pagingSettings)
        {
            PagingSettings = pagingSettings;
        }

        public void SetSortExpression(Func<IQueryable<T>, IOrderedQueryable<T>> sortExpression)
        {
            OrderBy = sortExpression;
        }

        public void SetNavigationProperties(params Expression<Func<T, object>>[] navigationProperties)
        {
            if (NavigationProperties != null) { NavigationProperties.Clear(); }
            AddNavigationProperties(navigationProperties);
        }

        public void AddNavigationProperties(params Expression<Func<T, object>>[] navigationProperties)
        {
            foreach (var prop in navigationProperties)
            {
                AddNavigationProperty(prop);
            }
        }

        public void AddIgnoreProperties(params Expression<Func<T, object>>[] ignoreProperties)
        {
            if (ignoreProperties != null)
            {
                foreach (var prop in ignoreProperties)
                {
                    AddIgnoreProperty(prop);
                }
            }
        }

        public void AddIgnoreProperty(Expression<Func<T, object>> ignoreProperty)
        {
            if (_ignoreProperties == null) { _ignoreProperties = new List<Expression<Func<T, object>>>(); }
            _ignoreProperties.Add(ignoreProperty);
        }

        public void AddNavigationProperty(Expression<Func<T, object>> navigationProperty)
        {
            if (NavigationProperties == null) { NavigationProperties = new List<Expression<Func<T, object>>>(); }
            NavigationProperties.Add(navigationProperty);
        }

        public virtual IList<T> GetAll()
        {
            List<T> list;
            using (var context = new ShoppingCartDbContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                // Apply eager loading
                if (NavigationProperties != null && NavigationProperties.Count > 0)
                {
                    foreach (Expression<Func<T, object>> navigationProperty in NavigationProperties)
                    {
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);
                    }
                }

                list = dbQuery
                  .AsNoTracking()
                  .ToList<T>();
            }


            return list;
        }

        public virtual IList<T> GetList(Expression<Func<T, bool>> where)
        {
            List<T> list;
            using (var context = new ShoppingCartDbContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                // Apply eager loading
                if (NavigationProperties != null && NavigationProperties.Count > 0)
                {
                    foreach (Expression<Func<T, object>> navigationProperty in NavigationProperties)
                    {
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);
                    }
                }

                dbQuery = dbQuery
                        .AsNoTracking()
                        .Where(where);

                // Set total number of records
                if (PagingSettings != null) { PagingSettings.TotalNumberOfRecords = dbQuery.Count(); }

                // Apply sorting
                dbQuery = OrderBy(dbQuery);

                // Retrieve data
                if (PagingSettings != null)
                {
                    dbQuery = dbQuery
                        .Skip((PagingSettings.Page - 1) * PagingSettings.PageSize)
                        .Take(PagingSettings.PageSize);
                }

                list = dbQuery.ToList<T>();

                // Set number of records
                if (PagingSettings != null) { PagingSettings.NumberOfRecords = list.Count(); }
            }

            return list;
        }

        public virtual int GetCount(Expression<Func<T, bool>> where)
        {
            int count;
            using (var context = new ShoppingCartDbContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                dbQuery = dbQuery.AsNoTracking();

                if (where != null)
                    dbQuery = dbQuery.Where(where);

                // Set total number of records
                count = dbQuery.Count();
            }
            return count;
        }

        public virtual bool Exists(Guid id)
        {
            bool exists = false;
            using (var context = new ShoppingCartDbContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                exists = dbQuery
                    .AsNoTracking()
                    .Any(obj => obj.Id == id);
            }

            return exists;
        }

        public virtual T GetFirst()
        {
            T item = null;
            using (var context = new ShoppingCartDbContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                item = dbQuery
                    .AsNoTracking()
                    .FirstOrDefault();
            }

            return item;
        }

        public virtual T GetLastCreated()
        {
            T item = null;
            using (var context = new ShoppingCartDbContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                item = dbQuery
                    .AsNoTracking()
                    .OrderByDescending(obj => obj.DateCreated)
                    .FirstOrDefault();
            }

            return item;
        }

        public virtual T GetSingle(Expression<Func<T, bool>> where)
        {
            T item = null;
            using (var context = new ShoppingCartDbContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                // Apply eager loading
                if (NavigationProperties != null && NavigationProperties.Count > 0)
                {
                    foreach (Expression<Func<T, object>> navigationProperty in NavigationProperties)
                    {
                        dbQuery = dbQuery.Include<T, object>(navigationProperty);
                    }
                }

                item = dbQuery
                    .AsNoTracking()
                    .FirstOrDefault(where);
            }

            return item;
        }

        public virtual void Add(params T[] items)
        {
            using (var context = new ShoppingCartDbContext())
            {
                foreach (T item in items)
                {
                    if (item.Id == Guid.Empty)
                    {
                        item.Id = Guid.NewGuid();
                    }

                    item.DateCreated = DateTime.Now;
                    item.DateModified = DateTime.Now;


                    context.Entry(item).State = EntityState.Added;
                }
                context.SaveChanges();
            }
        }

        public virtual void Update(params T[] items)
        {
            using (var context = new ShoppingCartDbContext())
            {
                foreach (T item in items)
                {
                    item.DateModified = DateTime.Now;

                    T attachedEntity = context.Set<T>().SingleOrDefault(e => e.Id == item.Id);
                    if (attachedEntity != null)
                    {
                        var entry = context.Entry(attachedEntity);
                        entry.CurrentValues.SetValues(item);

                        // Attach user entity and set all properties as modified
                        entry.State = EntityState.Modified;

                        if (_ignoreProperties != null && _ignoreProperties.Count > 0)
                        {
                            foreach (var ignoreProperty in _ignoreProperties)
                            {
                                entry.Property(ignoreProperty).IsModified = false;
                            }
                        }

                        entry.Property(i => i.DateCreated).IsModified = false;
                        entry.Property(i => i.CreatedBy).IsModified = false;
                    }
                }

                context.SaveChanges();
            }
        }

        public virtual void Delete(Guid id)
        {
            var itemToRemove = GetSingle(i => i.Id == id);
            Delete(itemToRemove);
        }

        public virtual void Delete(params T[] items)
        {
            using (var context = new ShoppingCartDbContext())
            {
                foreach (T item in items)
                {
                    context.Entry(item).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }
    }
}
