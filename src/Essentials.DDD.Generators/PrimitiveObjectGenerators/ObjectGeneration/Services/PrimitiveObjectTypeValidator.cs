using Microsoft.CodeAnalysis;
using Essentials.DDD.Generators.PrimitiveObjectGenerators.Dictionaries;
// ReSharper disable UseNameOfInsteadOfTypeOf

namespace Essentials.DDD.Generators.PrimitiveObjectGenerators.ObjectGeneration.Services;

/// <summary>
/// Сервис проверки типа на возможность его использования для работы с примитивным объектом
/// </summary>
internal static class PrimitiveObjectTypeValidator
{
    private static readonly HashSet<string> _supportedTypes = new(
        [
            "ushort",
            "short",
            "uint",
            "int",
            "ulong",
            "long",
            "float",
            "double",
            "decimal"
        ],
        StringComparer.OrdinalIgnoreCase);
    
    public static void Validate(INamedTypeSymbol primitiveObjectType)
    {
        var fullTypeName = primitiveObjectType.ToDisplayString();
        if (_supportedTypes.Contains(fullTypeName))
            return;
        
        throw new NotSupportedException(
            $"Тип данных '{fullTypeName}' не поддерживается для использования в атрибуте " +
            $"'{KnownAttributes.PRIMITIVE_OBJECT_ATTRIBUTE}'");
    }
}