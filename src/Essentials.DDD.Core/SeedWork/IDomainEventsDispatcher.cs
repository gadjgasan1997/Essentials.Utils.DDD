namespace Essentials.DDD.SeedWork;

/// <summary>
/// Обработчик доменных событий
/// </summary>
public interface IDomainEventsDispatcher
{
    /// <summary>
    /// Обрабатывает доменные события
    /// </summary>
    /// <param name="domainEvents">Доменные события</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Задача</returns>
    Task Dispatch(IDomainEvent[] domainEvents, CancellationToken token);
}