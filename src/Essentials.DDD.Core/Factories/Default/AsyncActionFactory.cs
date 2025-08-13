using LanguageExt;
using LanguageExt.Common;
using Ardalis.GuardClauses;

namespace Essentials.DDD.Factories.Default;

/// <summary>
/// Фабрика, принимающая в конструктор действие в формате делегата
/// </summary>
/// <typeparam name="TParameters">Тип параметров</typeparam>
/// <typeparam name="TResult">Тип результата</typeparam>
public class AsyncActionFactory<TParameters, TResult> : IAsyncFactory<TParameters, TResult>
    where TParameters : IParameters<TResult>
{
    private readonly Func<TParameters, CancellationToken, Task<Validation<Error, TResult>>> _func;
    
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="func">Действие создания объекта</param>
    public AsyncActionFactory(Func<TParameters, CancellationToken, Task<Validation<Error, TResult>>> func)
    {
        _func = Guard.Against.Null(func);
    }
    
    /// <inheritdoc cref="IAsyncFactory{TParameters, TResult}.CreateAsync" />
    public Task<Validation<Error, TResult>> CreateAsync(TParameters parameters, CancellationToken token) =>
        _func(parameters, token);
}