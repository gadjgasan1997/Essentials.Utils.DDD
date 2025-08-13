using Microsoft.CodeAnalysis;

// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.DDD.Generators.Extensions;

/// <summary>
/// Методы расширения для <see cref="Compilation" />
/// </summary>
internal static class CompilationExtensions
{
    public static INamedTypeSymbol GetRequiredType(this Compilation compilation, string fullTypeName)
    {
        var type = compilation.GetTypeByMetadataName(fullTypeName);
        return type ?? throw new KeyNotFoundException($"Не найден тип с названием '{fullTypeName}'");
    }
    
    public static string GetRequiredTypeName(this Compilation compilation, string fullTypeName) =>
        compilation.GetRequiredType(fullTypeName).ToFullyQualifiedString();
    
    public static INamedTypeSymbol GetRequiredGenericType(
        this Compilation compilation,
        string fullTypeName,
        params ITypeSymbol[] typeArguments)
    {
        var type = compilation.GetRequiredType(fullTypeName);
        return type.Construct(typeArguments);
    }
    
    public static string GetRequiredGenericTypeName(
        this Compilation compilation,
        string fullTypeName,
        params ITypeSymbol[] typeArguments)
    {
        return compilation.GetRequiredGenericType(fullTypeName, typeArguments).ToFullyQualifiedString();
    }
    
    #region Concrete Types
    
    public static ITypeSymbol GetBooleanType(this Compilation compilation) =>
        compilation.GetRequiredType("System.Boolean");
    
    #endregion
}