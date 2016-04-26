using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Models.Log
{
    public class LoggingAttribute : Attribute
    {

    }

    public class LoggingClassAttribute : Attribute
    {

    }

    public class ChangeTrackingService<T> where T : BaseObject
    {
        public List<ChangeTracking> GetChanges(T oldEntry, T newEntry)
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
                var oldValue = oldProperty.GetValue(oldEntry).ToString();
                Type t = Type.GetType(matchingProperty.PropertyType.FullName);
                var newValue = matchingProperty.GetValue(newEntry).ToString();


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
                        Description = "Update " + className + " " + matchingProperty.Name,
                        NewValue = matchingProperty.GetValue(newEntry).ToString(),
                        Type = ChangeType.Edit
                    });
                }
            }

            return logs;
        }

        public ChangeTracking AddingChange(T newEntry)
        {
            var newType = newEntry.GetType();

            var attributes = newType.GetCustomAttributes(typeof(LoggingClassAttribute), false);

            if (attributes.Length > 0)
            {
                ChangeTracking log = new ChangeTracking()
                {
                    Id = Guid.NewGuid(),
                    DateChanged = DateTime.Now,
                    ClassName = newEntry.GetType().Name,
                    PrimaryKey = newEntry.Id,
                    Description = "Adding New " + newEntry.GetType().Name,
                    Type = ChangeType.Add
                };

                return log;
            }

            return null;
        }

        public ChangeTracking DeleteChange(T newEntry)
        {
            var newType = newEntry.GetType();
            if (newType.BaseType.Name != "BaseObject")
                newType = Type.GetType(newType.BaseType.FullName);

            var attributes = newType.GetCustomAttributes(typeof(LoggingClassAttribute), false);

            if (attributes.Length > 0)
            {
                ChangeTracking log = new ChangeTracking()
                {
                    Id = Guid.NewGuid(),
                    DateChanged = DateTime.Now,
                    ClassName = newType.Name,
                    PrimaryKey = newEntry.Id,
                    Description = "Delete " + newEntry.GetType().Name,
                    Type = ChangeType.Delete
                };

                return log;
            }

            return null;
        }
    }
}
