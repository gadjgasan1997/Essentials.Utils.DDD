// ReSharper disable UnusedTypeParameter

namespace Essentials.DDD.Generators.PrimitiveObjectGenerators;

/// <summary>
/// Генерирует конвертер Newtonsoft.Json для модели
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class NewtonsoftJsonSerializableAttribute<T> : Attribute
    where T : struct
{
    /// <summary>
    /// Пространство имен, в которое будет добавлен сгенерированный конвертер
    /// </summary>
    public string? Namespace { get; set; }
}