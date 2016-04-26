using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Models.Log
{
    public class LoggingAttribute : Attribute
    {

    }

    public class ChangeTrackingService<T> where T : BaseObject
    {
        public List<ChangeTracking> GetChanges(T oldEntry, T newEntry, string description)
        {
            List<ChangeTracking> logs = new List<ChangeTracking>();

            var oldType = oldEntry.GetType();
            var newType = newEntry.GetType();
            /*if (oldType != newType)
            {
                return logs; //Types don't match, cannot log changes
            }*/

            var oldProperties = oldType.GetProperties();
            var newProperties = newType.GetProperties();

            var dateChanged = DateTime.Now;
            Guid primaryKey = oldEntry.Id;
            var className = oldType.BaseType.Name;

            foreach (var oldProperty in oldProperties)
            {
                var matchingProperty = newProperties.Where(x => Attribute.IsDefined(x, typeof(LoggingAttribute))
                                                                && x.Name == oldProperty.Name
                                                                && x.PropertyType == oldProperty.PropertyType)
                                                    .FirstOrDefault();
                if (matchingProperty == null)
                {
                    continue;
                }
                var oldValue = oldProperty.GetValue(oldEntry);
                var newValue = matchingProperty.GetValue(newEntry);
                if (matchingProperty != null && oldValue != newValue)
                {
                    logs.Add(new ChangeTracking()
                    {
                        Id = Guid.NewGuid(),
                        PrimaryKey = primaryKey,
                        DateChanged = dateChanged,
                        ClassName = className,
                        PropertyName = matchingProperty.Name,
                        OldValue = oldProperty.GetValue(oldEntry).ToString(),
                        Description = description,
                        NewValue = matchingProperty.GetValue(newEntry).ToString(),
                    });
                }
            }

            return logs;
        }

    }
}
