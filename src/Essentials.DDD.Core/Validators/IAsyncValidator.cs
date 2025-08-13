using LanguageExt;
using LanguageExt.Common;
using Essentials.DDD.SeedWork;

namespace Essentials.DDD.Validators;

/// <summary>
/// Валидатор
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public interface IAsyncValidator<in T> : ITransientService
{
    /// <summary>
    /// Проверяет сущность
    /// </summary>
    /// <param name="value">Сущность</param>
    /// <param name="token">Токен отмены</param>
    /// <returns></returns>
    Task<Validation<Error, Unit>> ValidateAsync(T value, CancellationToken token);
}