using System.Reflection;
using System.Collections.Concurrent;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StaticMemberInGenericType

namespace Essentials.DDD.SeedWork;

/// <summary>
/// Перечисление
/// </summary>
public record Enumeration : Enumeration<string>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="value">Строка</param>
    protected Enumeration(string value) : base(value)
    { }
}

/// <summary>
/// Перечисление
/// </summary>
public record Enumeration<T>
    where T : notnull
{
    private static readonly ConcurrentDictionary<object, Lazy<IEnumerable<object>>> _knownValues = [];
    
    /// <summary>
    /// Значение
    /// </summary>
    public T Value { get; }
    
    /// <summary>
    /// Возвращает все имеющиеся значения перечисления
    /// </summary>
    /// <typeparam name="TEnumeration">Тип перечисления</typeparam>
    /// <returns>Значения перечисления</returns>
    public static IEnumerable<TEnumeration> GetAll<TEnumeration>()
        where TEnumeration : Enumeration<T>
    {
        var values = _knownValues
            .GetOrAdd(
                typeof(TEnumeration),
                _ => new Lazy<IEnumerable<object>>(() =>
                {
                    return typeof(TEnumeration)
                        .GetProperties(BindingFlags.Public | BindingFlags.Static)
                        .Select(info => info.GetValue(null))
                        .Cast<object>();
                }))
            .Value;
        
        return values.Cast<TEnumeration>();
    }
    
    /// <summary>
    /// Преобразует значение в существующий объект перечисления, объявленный как публичное статичное свойство
    /// </summary>
    /// <param name="value">Значение</param>
    /// <typeparam name="TEnumeration">Тип перечисления</typeparam>
    /// <returns>Перечисление</returns>
    public static TEnumeration From<TEnumeration>(T value)
        where TEnumeration : Enumeration<T>
    {
        return GetAll<TEnumeration>().First(enumeration => enumeration.Value.Equals(value));
    }
    
    /// <summary>
    /// Неявный каст к значению
    /// </summary>
    /// <param name="enumeration">Перечисление</param>
    /// <returns>Значение</returns>
    public static implicit operator T(Enumeration<T> enumeration) => enumeration.Value;
    
    /// <summary>
    /// Неявный каст к объекту
    /// </summary>
    /// <param name="value">Значение</param>
    /// <returns>Перечисление</returns>
    public static implicit operator Enumeration<T>(T value) => new(value);
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="value">Значение</param>
    protected Enumeration(T value)
    {
        Value = value;
    }
}