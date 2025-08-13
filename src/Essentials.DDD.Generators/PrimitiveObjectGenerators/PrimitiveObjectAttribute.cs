namespace Essentials.DDD.Generators.PrimitiveObjectGenerators;

/// <summary>
/// Атрибут для генерации примитивных объектов
/// </summary>
/// <typeparam name="T">Тип объекта</typeparam>
// ReSharper disable UnusedTypeParameter
[AttributeUsage(AttributeTargets.Struct)]
public class PrimitiveObjectAttribute<T> : Attribute where T : struct
{
    /// <summary>
    /// Признак необходимости генерации операторов неявного преобразования
    /// </summary>
    public bool GenerateImplicitOperators { get; set; }
    
    /// <summary>
    /// Признак необходимости выполнения трансформации перед созданием сущности (выполняется после валидации)
    /// </summary>
    public bool NeedTransform { get; set; }
    
    /// <summary>
    /// Признак необходимости явной реализации метода Equals
    /// </summary>
    public bool ExplicitEquals { get; set; }
    
    /// <summary>
    /// Признак необходимости явной реализации метода CompareTo
    /// </summary>
    public bool ExplicitCompareTo { get; set; }
}