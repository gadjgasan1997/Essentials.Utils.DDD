using LanguageExt;
using LanguageExt.Common;
using Ardalis.GuardClauses;

namespace Essentials.DDD.Factories.Default;

/// <summary>
/// Фабрика, принимающая в конструктор действие в формате делегата
/// </summary>
/// <typeparam name="TParameters">Тип параметров</typeparam>
/// <typeparam name="TResult">Тип результата</typeparam>
public class ActionFactory<TParameters, TResult> : IFactory<TParameters, TResult>
    where TParameters : IParameters<TResult>
{
    private readonly Func<TParameters, Validation<Error, TResult>> _func;
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="func">Действие создания объекта</param>
    public ActionFactory(Func<TParameters, Validation<Error, TResult>> func)
    {
        _func = Guard.Against.Null(func);
    }
    
    /// <inheritdoc cref="IFactory{TParameters, TResult}.Create" />
    public Validation<Error, TResult> Create(TParameters parameters) => _func(parameters);
}