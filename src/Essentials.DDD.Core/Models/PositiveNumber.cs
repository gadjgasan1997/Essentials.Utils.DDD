using System.Numerics;
// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.DDD.Models;

/// <summary>
/// Положительное число
/// </summary>
/// <typeparam name="T">Тип числа</typeparam>
public readonly record struct PositiveNumber<T>
    where T : INumber<T>
{
    private readonly T _value;
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="value">Значение</param>
    /// <exception cref="ArgumentException">Исключение, выбрасываемое в случае невалидного значения</exception>
    public PositiveNumber(T value)
    {
        if (ValueIsInvalid(value))
        {
            throw new ArgumentException(
                $"Значение для создания экземпляра '{typeof(T).FullName}' должно быть строго положительное. " +
                $"Значение: '{value}'",
                nameof(value));
        }
        
        _value = value;
    }
    
    /// <summary>
    /// Значение
    /// </summary>
    public T Value => ValueIsInvalid(_value)
        ? throw new InvalidOperationException(
            $"Объект '{typeof(T).FullName}' был сконструирован неверно. Значение: '{_value}'")
        : _value;
    
    /// <summary>
    /// Преобразует число в тип <typeparamref name="TResult" />
    /// </summary>
    /// <typeparam name="TResult">Тип результирующего числа</typeparam>
    /// <returns>Положительное число</returns>
    public PositiveNumber<TResult> Cast<TResult>()
        where TResult : INumber<TResult>
    {
        var result = TResult.CreateChecked(Value);
        return new PositiveNumber<TResult>(result);
    }
    
    /// <summary>
    /// Оператор преобразования значения в объект
    /// </summary>
    /// <param name="value">Значение</param>
    /// <returns>Положительное число</returns>
    public static implicit operator PositiveNumber<T>(T value) => new(value);
    
    /// <summary>
    /// Оператор преобразования числа в значение
    /// </summary>
    /// <param name="positiveNumber">Положительное число</param>
    /// <returns>Положительное число</returns>
    public static implicit operator T(PositiveNumber<T> positiveNumber) => positiveNumber.Value;
    
    /// <summary>
    /// Определяет, что значение является невалидным
    /// </summary>
    /// <param name="value">Значение</param>
    /// <returns>Признак</returns>
    private static bool ValueIsInvalid(T value) => T.IsNegative(value) || T.IsZero(value);
}