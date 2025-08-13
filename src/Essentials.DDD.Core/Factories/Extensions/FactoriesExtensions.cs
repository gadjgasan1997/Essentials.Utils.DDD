using LanguageExt;
using LanguageExt.Common;
using Essentials.DDD.Factories.Parameters;

namespace Essentials.DDD.Factories.Extensions;

/// <summary>
/// Методы расширения для работы с фабриками
/// </summary>
public static class FactoriesExtensions
{
    /// <summary>
    /// Создает сущность
    /// </summary>
    /// <param name="factory">Фабрика</param>
    /// <param name="value">Значение</param>
    /// <typeparam name="TValue">Тип значения</typeparam>
    /// <typeparam name="TResult">Тип результата</typeparam>
    /// <returns></returns>
    public static Validation<Error, TResult> Create<TValue, TResult>(
        this IFactory<ValueParameters<TValue, TResult>, TResult> factory,
        TValue? value)
    {
        if (value is null)
        {
            return Error.New(
                $"Параметр '{typeof(TValue).FullName}' для создания объекта " +
                $"'{typeof(TResult).FullName}' не может быть пустым");
        }
        
        return factory.Create(new ValueParameters<TValue, TResult>(value));
    }
}