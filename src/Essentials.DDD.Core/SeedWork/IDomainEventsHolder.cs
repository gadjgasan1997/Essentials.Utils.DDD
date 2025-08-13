namespace Essentials.DDD.SeedWork;

/// <summary>
/// Холдер доменных событий
/// </summary>
public interface IDomainEventsHolder
{
    /// <summary>
    /// Возвращает доменные события
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    
    /// <summary>
    /// Очищает доменные события
    /// </summary>
    void ClearDomainEvents();
}