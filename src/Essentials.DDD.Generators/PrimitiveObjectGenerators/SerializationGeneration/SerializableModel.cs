using Microsoft.CodeAnalysis;

namespace Essentials.DDD.Generators.PrimitiveObjectGenerators.SerializationGeneration;

/// <summary>
/// Модель с опциями сериализации
/// </summary>
internal class SerializableModel
{
    public SerializableModel(
        Compilation compilation,
        string @namespace,
        INamedTypeSymbol structType,
        INamedTypeSymbol primitiveObjectType)
    {
        Compilation = compilation;
        Namespace = @namespace;
        StructType = structType;
        PrimitiveObjectType = primitiveObjectType;
    }
    
    public Compilation Compilation { get; }
    
    public string Namespace { get; }
    
    public INamedTypeSymbol StructType { get; }
    
    public string StructTypeShortName => StructType.Name;
    
    public string StructTypeGlobalName => $"global::{StructType.ToDisplayString()}";
    
    public INamedTypeSymbol PrimitiveObjectType { get; }
    
    public string PrimitiveObjectTypeName => $"{PrimitiveObjectType.ContainingNamespace.Name}.{PrimitiveObjectType.Name}";
}

internal class SerializableModelComparer : IEqualityComparer<SerializableModel>
{
    public static SerializableModelComparer Instance { get; } = new();
    
    public bool Equals(SerializableModel? x, SerializableModel? y)
    {
        if (x is null && y is null)
            return true;
        
        if (x is null || y is null)
            return false;
        
        return string.Equals(x.StructTypeShortName, y.StructTypeShortName, StringComparison.Ordinal);
    }
    
    public int GetHashCode(SerializableModel obj) => obj.StructTypeShortName.GetHashCode();
}