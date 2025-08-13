using Microsoft.CodeAnalysis;

namespace Essentials.DDD.Generators.Extensions;

/// <summary>
/// Методы расширения для <see cref="ISymbol" />
/// </summary>
internal static class SymbolExtensions
{
    /// <summary>
    /// Возвращает название типа в полном формате
    /// </summary>
    /// <param name="symbol">Тип</param>
    /// <param name="symbolDisplayGlobalNamespaceStyle">Стиль отображения пространств имен</param>
    /// <param name="symbolDisplayGenericsOptions">Стиль отображения generic типов</param>
    /// <returns>Название</returns>
    public static string ToFullyQualifiedString(
        this ISymbol symbol,
        SymbolDisplayGlobalNamespaceStyle symbolDisplayGlobalNamespaceStyle = SymbolDisplayGlobalNamespaceStyle.Included,
        SymbolDisplayGenericsOptions symbolDisplayGenericsOptions = SymbolDisplayGenericsOptions.IncludeTypeParameters)
    {
        return symbol.ToDisplayString(
            SymbolDisplayFormat
                .FullyQualifiedFormat
                .WithGlobalNamespaceStyle(symbolDisplayGlobalNamespaceStyle)
                .WithGenericsOptions(symbolDisplayGenericsOptions));
    }
}