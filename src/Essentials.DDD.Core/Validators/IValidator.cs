using LanguageExt;
using LanguageExt.Common;
using Essentials.DDD.SeedWork;

namespace Essentials.DDD.Validators;

/// <summary>
/// Валидатор
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public interface IValidator<in T> : ITransientService
{
    /// <summary>
    /// Проверяет сущность
    /// </summary>
    /// <param name="value">Сущность</param>
    /// <returns></returns>
    Validation<Error, Unit> Validate(T value);
}