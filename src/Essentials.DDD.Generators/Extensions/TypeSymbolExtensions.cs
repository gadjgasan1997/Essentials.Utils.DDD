using Microsoft.CodeAnalysis;

namespace Essentials.DDD.Generators.Extensions;

/// <summary>
/// Методы расширения для работы с <see cref="ITypeSymbol" />
/// </summary>
internal static class TypeSymbolExtensions
{
    public static ITypeSymbol? GetInterfaceSymbolByFullName(
        this ITypeSymbol symbol,
        string fullTypeName)
    {
        var globalName = $"global::{fullTypeName}";
        
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var interfaceSymbol in symbol.AllInterfaces)
        {
            var interfaceSymbolName = interfaceSymbol.ToFullyQualifiedString(
                symbolDisplayGenericsOptions: SymbolDisplayGenericsOptions.None);
            
            if (interfaceSymbolName == globalName)
                return interfaceSymbol;
        }
        
        return null;
    }
    
    
}