using System.Text;

namespace Essentials.DDD.SeedWork;

/// <summary>
/// Value Object
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Возвращает компоненты, на основании которых будет определяться равенство объектов
    /// </summary>
    /// <returns>Компоненты</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();
    
    /// <summary>
    /// Определяет, отличается ли объект <paramref name="obj" /> от текущего
    /// </summary>
    /// <param name="obj">Объект</param>
    /// <returns>Признак</returns>
    public bool NotEquals(object? obj) => !Equals(obj);
    
    /// <summary>
    /// Определяет, равен ли объект <paramref name="obj" /> текущему
    /// </summary>
    /// <param name="obj">Объект</param>
    /// <returns>Признак</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;
        
        var other = (ValueObject) obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }
    
    /// <see cref="object.GetHashCode" />
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate(36, HashCode.Combine);
    }
    
    /// <see cref="object.ToString" />
    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var component in GetEqualityComponents())
        {
            if (component is null)
                continue;
            
            builder.Append(", ");
            builder.Append(component);
        }
        
        return builder.ToString();
    }
    
    /// <summary>
    /// Оператор равенства
    /// </summary>
    /// <param name="left">Левое значение</param>
    /// <param name="right">Правое значение</param>
    /// <returns>Признак равенства</returns>
    public static bool operator ==(ValueObject left, ValueObject right) => Equals(left, right);
    
    /// <summary>
    /// Оператор равенства
    /// </summary>
    /// <param name="left">Левое значение</param>
    /// <param name="right">Правое значение</param>
    /// <returns>Признак равенства</returns>
    public static bool operator !=(ValueObject left, ValueObject right) => !Equals(left, right);
}