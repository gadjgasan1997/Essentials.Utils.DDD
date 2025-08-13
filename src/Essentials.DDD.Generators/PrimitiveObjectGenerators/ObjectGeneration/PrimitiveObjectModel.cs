using Microsoft.CodeAnalysis;

namespace Essentials.DDD.Generators.PrimitiveObjectGenerators.ObjectGeneration;

/// <summary>
/// Модель примитивного объекта для генерации
/// </summary>
internal class PrimitiveObjectModel
{
    public PrimitiveObjectModel(
        Compilation compilation,
        string @namespace,
        INamedTypeSymbol structType,
        INamedTypeSymbol primitiveObjectType,
        bool generateImplicitOperators,
        bool needTransform,
        bool needExplicitEquals,
        bool needExplicitCompareTo)
    {
        Compilation = compilation;
        Namespace = @namespace;
        StructType = structType;
        PrimitiveObjectType = primitiveObjectType;
        GenerateImplicitOperators = generateImplicitOperators;
        NeedTransform = needTransform;
        NeedExplicitEquals = needExplicitEquals;
        NeedExplicitCompareTo = needExplicitCompareTo;
    }
    
    public Compilation Compilation { get; }
    
    public string Namespace { get; }
    
    public string Accessibility => StructType.DeclaredAccessibility.ToString().ToLower();
    
    public INamedTypeSymbol StructType { get; }
    
    public string StructTypeShortName => StructType.Name;
    
    public string StructTypeGlobalName => $"global::{StructType.ToDisplayString()}";
    
    public INamedTypeSymbol PrimitiveObjectType { get; }
    
    public string PrimitiveObjectTypeName => $"{PrimitiveObjectType.ContainingNamespace.Name}.{PrimitiveObjectType.Name}";
    
    public bool GenerateImplicitOperators { get; }
    
    public bool NeedTransform { get; }
    
    public bool NeedExplicitEquals { get; }
    
    public bool NeedExplicitCompareTo { get; }
}

internal class PrimitiveObjectModelComparer : IEqualityComparer<PrimitiveObjectModel>
{
    public static PrimitiveObjectModelComparer Instance { get; } = new();
    
    public bool Equals(PrimitiveObjectModel? x, PrimitiveObjectModel? y)
    {
        if (x is null && y is null)
            return true;
        
        if (x is null || y is null)
            return false;
        
        return string.Equals(x.StructTypeShortName, y.StructTypeShortName, StringComparison.Ordinal);
    }
    
    public int GetHashCode(PrimitiveObjectModel obj) => obj.StructTypeShortName.GetHashCode();
}