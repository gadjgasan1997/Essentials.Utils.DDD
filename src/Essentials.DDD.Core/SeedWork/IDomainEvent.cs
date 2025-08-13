namespace Essentials.DDD.SeedWork;

/// <summary>
/// Доменное событие
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Отправитель события
    /// </summary>
    string Sender { get; }
    
    /// <summary>
    /// Дата и время возникновения события
    /// </summary>
    DateTime OccurredIn { get; }
}