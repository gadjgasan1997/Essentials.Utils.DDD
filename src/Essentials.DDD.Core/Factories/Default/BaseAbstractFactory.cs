using LanguageExt;
using LanguageExt.Common;
using static Essentials.DDD.Factories.FactoriesStorage;
// ReSharper disable ConvertIfStatementToReturnStatement

namespace Essentials.DDD.Factories.Default;

/// <summary>
/// Базовая абстрактная фабрика
/// </summary>
/// <typeparam name="TResult">Тип результата</typeparam>
public abstract class BaseAbstractFactory<TResult> : IAbstractFactory<TResult>
{
    /// <summary>
    /// Инициализирует фабрики объектов
    /// </summary>
    protected abstract void InitFactories();
    
    /// <summary>
    /// Добавляет фабрику объекта
    /// </summary>
    /// <param name="factory">Фабрика</param>
    /// <typeparam name="TParameters">Тип параметров</typeparam>
    /// <typeparam name="TFactory">Тип фабрики</typeparam>
    protected void AddFactory<TParameters, TFactory>(TFactory factory)
        where TParameters : IParameters<TResult>
        where TFactory : IFactory<TParameters, TResult>
    {
        FactoriesMap[typeof(TParameters)] = factory;
    }
    
    /// <summary>
    /// Добавляет фабрику объекта
    /// </summary>
    /// <param name="func">Фабрика</param>
    /// <typeparam name="TParameters">Тип параметров</typeparam>
    protected void AddFactory<TParameters>(Func<TParameters, Validation<Error, TResult>> func)
        where TParameters : IParameters<TResult>
    {
        FactoriesMap[typeof(TParameters)] = new ActionFactory<TParameters, TResult>(func);
    }
    
    /// <summary>
    /// Добавляет фабрику объекта
    /// </summary>
    /// <param name="factory">Фабрика</param>
    /// <typeparam name="TParameters">Тип параметров</typeparam>
    /// <typeparam name="TFactory">Тип фабрики</typeparam>
    protected void AddAsyncFactory<TParameters, TFactory>(TFactory factory)
        where TParameters : IParameters<TResult>
        where TFactory : IAsyncFactory<TParameters, TResult>
    {
        FactoriesMap[typeof(TParameters)] = factory;
    }
    
    /// <summary>
    /// Добавляет фабрику объекта
    /// </summary>
    /// <param name="func">Фабрика</param>
    /// <typeparam name="TParameters">Тип параметров</typeparam>
    protected void AddAsyncFactory<TParameters>(Func<TParameters, CancellationToken, Task<Validation<Error, TResult>>> func)
        where TParameters : IParameters<TResult>
    {
        FactoriesMap[typeof(TParameters)] = new AsyncActionFactory<TParameters, TResult>(func);
    }
    
    /// <inheritdoc cref="IAbstractFactory{TResult}.CreateFactory{TParameters}" />
    public IFactory<TParameters, TResult> CreateFactory<TParameters>()
        where TParameters : IParameters<TResult>
    {
        if (FactoriesMap.TryGetValue(typeof(TParameters), out var existingFactory))
            return (IFactory<TParameters, TResult>) existingFactory;
        
        InitFactoriesCore();
        
        if (FactoriesMap.TryGetValue(typeof(TParameters), out var newFactory))
            return (IFactory<TParameters, TResult>) newFactory;
        
        throw new UnsupportedParametersTypeException<TParameters>();
    }
    
    /// <inheritdoc cref="IAbstractFactory{TResult}.CreateAsyncFactory{TParameters}" />
    public IAsyncFactory<TParameters, TResult> CreateAsyncFactory<TParameters>()
        where TParameters : IParameters<TResult>
    {
        if (FactoriesMap.TryGetValue(typeof(TParameters), out var existingFactory))
            return (IAsyncFactory<TParameters, TResult>) existingFactory;
        
        InitFactoriesCore();
        
        if (FactoriesMap.TryGetValue(typeof(TParameters), out var newFactory))
            return (IAsyncFactory<TParameters, TResult>) newFactory;
        
        throw new UnsupportedParametersTypeException<TParameters>();
    }
    
    private void InitFactoriesCore()
    {
        lock (FactoriesLocker)
        {
            if (FactoriesAreConfigured)
                return;
            
            InitFactories();
            FactoriesAreConfigured = true;
        }
    }
    
    private sealed class UnsupportedParametersTypeException<TParameters> : KeyNotFoundException
    {
        public UnsupportedParametersTypeException()
            : base(
                $"Неподдерживаемый тип параметров для создания объекта '{typeof(TResult).FullName}'. " +
                $"Тип параметров: '{typeof(TParameters).FullName}'")
        { }
    }
}