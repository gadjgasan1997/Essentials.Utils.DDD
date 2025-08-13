namespace Essentials.DDD.SeedWork;

/// <summary>
/// Сервис
/// </summary>
public interface IService;

/// <summary>
/// Сервис с временем жизни Transient
/// </summary>
public interface ITransientService : IService;

/// <summary>
/// Сервис с временем жизни Scoped
/// </summary>
public interface IScopedService : IService;

/// <summary>
/// Сервис с временем жизни Singleton
/// </summary>
public interface ISingletonService : IService;