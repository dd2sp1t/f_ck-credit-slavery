using F_ckCreditSlavery.Entities.DataTransferObjects;

namespace F_ckCreditSlavery.Contracts;

public interface IDataShaper<in T>
{
    IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString);
    ShapedEntity ShapeData(T entity, string fieldsString);
}