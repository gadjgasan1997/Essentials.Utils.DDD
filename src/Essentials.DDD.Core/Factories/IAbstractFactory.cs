using Essentials.DDD.SeedWork;

namespace Essentials.DDD.Factories;

/// <summary>
/// Абстрактная фабрика
/// </summary>
/// <typeparam name="TResult">Тип объекта</typeparam>
public interface IAbstractFactory<TResult> : ITransientService
{
    /// <summary>
    /// Создает фабрику объекта по типу параметров
    /// </summary>
    /// <typeparam name="TParameters">Тип параметров</typeparam>
    /// <returns>Фабрика объекта</returns>
    IFactory<TParameters, TResult> CreateFactory<TParameters>()
        where TParameters : IParameters<TResult>;
    
    /// <summary>
    /// Создает фабрику объекта по типу параметров
    /// </summary>
    /// <typeparam name="TParameters">Тип параметров</typeparam>
    /// <returns>Фабрика объекта</returns>
    IAsyncFactory<TParameters, TResult> CreateAsyncFactory<TParameters>()
        where TParameters : IParameters<TResult>;
}