namespace Essentials.DDD.Factories.Parameters;

/// <summary>
/// Параметры с объектом <paramref name="Value" />
/// </summary>
/// <param name="Value">Значение</param>
public readonly record struct ValueParameters<TValue, TReturn>(TValue Value) : IParameters<TReturn>;