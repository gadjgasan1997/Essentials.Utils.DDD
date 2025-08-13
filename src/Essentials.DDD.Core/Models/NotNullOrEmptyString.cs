namespace Essentials.DDD.Models;

/// <summary>
/// Не пустая и не null строка
/// </summary>
public readonly record struct NotNullOrEmptyString
{
    private readonly string _value;
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="value">Значение</param>
    /// <exception cref="ArgumentException">Исключение, выбрасываемое в случае невалидного значения</exception>
    public NotNullOrEmptyString(string value)
    {
        if (ValueIsInvalid(value))
        {
            throw new ArgumentException(
                $"Значение для создания экземпляра '{typeof(NotNullOrEmptyString).FullName}' " +
                $"не может быть null или пустой строкой. Значение: '{value}'",
                nameof(value));
        }
        
        _value = value;
    }
    
    /// <summary>
    /// Значение
    /// </summary>
    public string Value => ValueIsInvalid(_value)
        ? throw new InvalidOperationException(
            $"Объект '{typeof(NotNullOrEmptyString).FullName}' был сконструирован неверно. " +
            $"Значение: '{_value}'")
        : _value;
    
    /// <inheritdoc cref="object.ToString" />
    public override string ToString() => Value;
    
    /// <summary>
    /// Оператор преобразования значения в объект
    /// </summary>
    /// <param name="value">Значение</param>
    /// <returns>Не пустая и не null строка</returns>
    public static implicit operator NotNullOrEmptyString(string value) => new(value);
    
    /// <summary>
    /// Оператор преобразования объекта в строку 
    /// </summary>
    /// <param name="value">Объект</param>
    /// <returns>Строка</returns>
    public static implicit operator string(NotNullOrEmptyString value) => value.Value;
    
    /// <summary>
    /// Определяет, что значение является невалидным
    /// </summary>
    /// <param name="value">Значение</param>
    /// <returns>Признак</returns>
    private static bool ValueIsInvalid(string value) => string.IsNullOrWhiteSpace(value);
}