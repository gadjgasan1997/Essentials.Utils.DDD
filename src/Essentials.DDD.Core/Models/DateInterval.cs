using Ardalis.GuardClauses;
using Essentials.DDD.SeedWork;

namespace Essentials.DDD.Models;

/// <summary>
/// Интервал дат
/// </summary>
public class DateInterval : ValueObject
{
    /// <summary>
    /// Начало интервала
    /// </summary>
    public DateOnly Start { get; }
    
    /// <summary>
    /// Конец интервала
    /// </summary>
    public DateOnly End { get; }
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="start">Начало интервала</param>
    /// <param name="end">Конец интервала</param>
    public DateInterval(DateOnly start, DateOnly end)
    {
        Guard.Against.Null(start, message: "Начало интервала не инициализировано");
        Guard.Against.Null(end, message: "Конец интервала не инициализирован");
        
        if (end <= start)
        {
            throw new ArgumentException(
                $"Дата окончания интервала ('{end}') " +
                $"не может быть меньше или равна даты начала интервала ('{start}')");
        }
        
        Start = start;
        End = end;
    }
    
    /// <inheritdoc cref="ValueObject.GetEqualityComponents" />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
    
    /// <inheritdoc cref="object.ToString" />
    public override string ToString() => $"{Start} - {End}";
}