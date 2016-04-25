using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Models.Models.Entities;
using System.Data.Entity;
using ShoppingCart.Models.Log;

namespace ShoppingCart.Models.Repositories.Concrete
{
    public class ProductRepository : GenericRepository<Product>
    {
        public override void Add(params Product[] items)
        {
            using (var context = new ShoppingCartDbContext())
            {
                foreach (Product item in items)
                {
                    if (item.Id == Guid.Empty)
                    {
                        item.Id = Guid.NewGuid();
                    }

                    item.DateCreated = DateTime.Now;
                    item.DateModified = DateTime.Now;

                    ChangeTracking logs = new ChangeTracking()
                    {
                        Id = Guid.NewGuid(),
                        DateChanged = DateTime.Now,
                        ClassName = item.GetType().Name,
                        PrimaryKey = item.Id,
                        Description = "Adding New Product"
                    };

                    context.ChangesTracking.Add(logs);

                    context.Products.Attach(item);
                    context.Products.Add(item);
                }
                context.SaveChanges();
            }
        }
        public override void Update(params Product[] items)
        {
            using (var context = new ShoppingCartDbContext())
            {
                foreach (Product item in items)
                {
                    item.DateModified = DateTime.Now;

                    Product attachedEntity = context.Set<Product>()
                        .Include(e => e.Providers)
                        .SingleOrDefault(e => e.Id == item.Id);

                    ChangeTrackingService<Product> service = new ChangeTrackingService<Product>();
                    List<ChangeTracking> logs = service.GetChanges(attachedEntity, item, "Update Product");

                    if (logs != null)
                    {
                        foreach (ChangeTracking log in logs)
                        {
                            context.ChangesTracking.Add(log);
                        }
                    }

                    var providers = new List<Provider>(item.Providers).ToList();

                    if (attachedEntity != null)
                    {
                        var entry = context.Entry(attachedEntity);
                        entry.CurrentValues.SetValues(item);

                        attachedEntity.Providers.Clear();

                        // Attach user entity and set all properties as modified
                        entry.State = EntityState.Modified;

                        if (_ignoreProperties != null && _ignoreProperties.Count > 0)
                        {
                            foreach (var ignoreProperty in _ignoreProperties)
                            {
                                entry.Property(ignoreProperty).IsModified = false;
                            }
                        }

                        foreach (var provider in context.Providers)
                        {
                            if (providers.Any(p => p.Id == provider.Id))
                            {
                                context.Providers.Attach(provider);
                                attachedEntity.Providers.Add(provider);
                            }
                            else
                            {
                                attachedEntity.Providers.Remove(provider);
                            }
                        }

                        entry.Property(i => i.DateCreated).IsModified = false;
                        entry.Property(i => i.CreatedBy).IsModified = false;
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
