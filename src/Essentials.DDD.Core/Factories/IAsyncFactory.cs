using LanguageExt;
using LanguageExt.Common;
using Essentials.DDD.SeedWork;

namespace Essentials.DDD.Factories;

/// <summary>
/// Фабрика для создания объекта типа <typeparamref typeparamref="TResult" />
/// </summary>
/// <typeparam name="TParameters">Тип входящих параметров</typeparam>
/// <typeparam name="TResult">Тип объекта</typeparam>
public interface IAsyncFactory<in TParameters, TResult> : ITransientService
    where TParameters : IParameters<TResult>
{
    /// <summary>
    /// Создает объект
    /// </summary>
    /// <param name="parameters">Параметры создания объекта</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Объект</returns>
    Task<Validation<Error, TResult>> CreateAsync(TParameters parameters, CancellationToken token);
}