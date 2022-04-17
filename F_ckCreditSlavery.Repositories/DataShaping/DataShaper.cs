using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using F_ckCreditSlavery.Contracts;
using F_ckCreditSlavery.Entities.DataTransferObjects;

namespace F_ckCreditSlavery.Repositories.DataShaping;

public class DataShaper<T> : IDataShaper<T> where T : class
{
    public PropertyInfo[] Properties { get; }

    public DataShaper()
    {
        Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString)
    {
        var requiredProperties = GetRequiredProperties(fieldsString);
        return FetchData(entities, requiredProperties);
    }

    public ShapedEntity ShapeData(T entity, string fieldsString)
    {
        var requiredProperties = GetRequiredProperties(fieldsString);
        return FetchDataForEntity(entity, requiredProperties);
    }

    private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
    {
        var requiredProperties = new List<PropertyInfo>();

        if (string.IsNullOrWhiteSpace(fieldsString))
        {
            requiredProperties = Properties.ToList();
        }
        else
        {
            var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var field in fields)
            {
                var property = Properties.FirstOrDefault(
                    info => info.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));

                if (property == null) continue;

                requiredProperties.Add(property);
            }
        }

        return requiredProperties;
    }

    private static IEnumerable<ShapedEntity> FetchData(
        IEnumerable<T> entities,
        IEnumerable<PropertyInfo> requiredProperties)
    {
        return (
            from entity in entities
            let propertyInfos = requiredProperties.ToList()
            select FetchDataForEntity(entity, propertyInfos)
        ).ToList();
    }

    private static ShapedEntity FetchDataForEntity(
        T entity,
        IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedObject = new ShapedEntity();

        foreach (var property in requiredProperties)
        {
            var objectPropertyValue = property.GetValue(entity);
            shapedObject.Entity.TryAdd(property.Name, objectPropertyValue);
        }

        var objectProperty = entity.GetType().GetProperty("Id");
        shapedObject.Id = (int) objectProperty!.GetValue(entity)!;

        return shapedObject;
    }
}