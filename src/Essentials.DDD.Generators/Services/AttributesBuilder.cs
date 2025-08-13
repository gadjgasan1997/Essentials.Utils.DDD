using Microsoft.CodeAnalysis;
using Essentials.DDD.Generators.Extensions;
using static Essentials.DDD.Generators.Dictionaries.KnownNamespaces;
// ReSharper disable MemberCanBePrivate.Global

namespace Essentials.DDD.Generators.Services;

/// <summary>
/// Билдер атрибутов
/// </summary>
internal static class AttributesBuilder
{
    public static string BuildMethodImplAttribute(Compilation compilation)
    {
        var attributeTypeName = compilation.GetRequiredTypeName($"{COMPILER_SERVICES}.MethodImplAttribute");
        var optionsTypeName = compilation.GetRequiredTypeName($"{COMPILER_SERVICES}.MethodImplOptions");
        return $"[{attributeTypeName}({optionsTypeName}.AggressiveInlining)]";
    }
    
    public static string BuildExcludeFromCodeCoverageAttribute(Compilation compilation)
    {
        const string justification = "Код является сгенерированным, " +
                                     "поэтому исключен из расчета процента покрытия тестами";
        
        var attributeTypeName = compilation.GetRequiredTypeName($"{CODE_ANALYSIS}.ExcludeFromCodeCoverageAttribute");
        return $"[{attributeTypeName}(Justification = \"{justification}\")]";
    }
    
    public static string BuildThreadStaticAttribute(Compilation compilation)
    {
        var attributeTypeName = compilation.GetRequiredTypeName($"{SYSTEM}.ThreadStaticAttribute");
        return $"[{attributeTypeName}]";
    }
    
    public static string BuildUnsafeFieldAccessorAttribute(Compilation compilation, string fieldName)
    {
        var attributeTypeName = compilation.GetRequiredTypeName($"{COMPILER_SERVICES}.UnsafeAccessorAttribute");
        var accessorKindTypeName = compilation.GetRequiredTypeName($"{COMPILER_SERVICES}.UnsafeAccessorKind");
        return $"[{attributeTypeName}({accessorKindTypeName}.Field, Name = \"{fieldName}\")]";
    }
}