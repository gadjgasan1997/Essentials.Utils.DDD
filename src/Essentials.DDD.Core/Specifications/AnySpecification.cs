using Ardalis.Specification;
using System.Linq.Expressions;

namespace Essentials.DDD.Specifications;

/// <summary>
/// Спецификация получения первой доступной записи
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public sealed class AnySpecification<T> : Specification<T>
    where T : class
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="joins">Список джойнов</param>
    public AnySpecification(params Expression<Func<T, object?>>[] joins)
    {
        foreach (var join in joins)
            Query.Include(join);
        
        Query.Where(_ => true);
    }
    
    /// <summary>
    /// Значение по-умолчанию
    /// </summary>
    public static AnySpecification<T> Default { get; } = new();
}