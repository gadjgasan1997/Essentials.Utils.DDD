using Ardalis.GuardClauses;
using Essentials.DDD.SeedWork;

namespace Essentials.DDD.Models;

/// <summary>
/// Временной интервал
/// </summary>
public class TimeInterval : ValueObject
{
    /// <summary>
    /// Начало интервала
    /// </summary>
    public TimeOnly Start { get; }
    
    /// <summary>
    /// Конец интервала
    /// </summary>
    public TimeOnly End { get; }
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="start">Начало интервала</param>
    /// <param name="end">Конец интервала</param>
    public TimeInterval(TimeOnly start, TimeOnly end)
    {
        Guard.Against.Null(start, message: "Начало интервала не инициализировано");
        Guard.Against.Null(end, message: "Конец интервала не инициализирован");
        
        if (end <= start)
        {
            throw new ArgumentException(
                $"Время окончания интервала ('{end}') " +
                $"не может быть меньше или равно времени начала интервала ('{start}')");
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