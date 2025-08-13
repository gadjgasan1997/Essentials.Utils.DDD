using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
// ReSharper disable ConvertIfStatementToReturnStatement

namespace Essentials.DDD.Generators.Extensions;

/// <summary>
/// Методы расширения для работы с атрибутами
/// </summary>
internal static class AttributesExtensions
{
    public static AttributeSyntax? GetAttributeSyntaxByFullTypeName(
        this SyntaxList<AttributeListSyntax> attributeListSyntax,
        SemanticModel semanticModel,
        string fullTypeName)
    {
        var globalName = $"global::{fullTypeName}";
        foreach (var attribute in attributeListSyntax.SelectMany(attributeList => attributeList.Attributes))
        {
            if (attribute.Name is not GenericNameSyntax genericNameSyntax)
                continue;
            
            var attributeSymbolInfo = semanticModel.GetSymbolInfo(genericNameSyntax);
            if (attributeSymbolInfo.Symbol is not IMethodSymbol attributeSymbol)
                continue;
            
            var attributeTypeName = attributeSymbol.ContainingType.ToDisplayString(
                SymbolDisplayFormat.FullyQualifiedFormat.WithGenericsOptions(
                    SymbolDisplayGenericsOptions.None));
            
            if (attributeTypeName != globalName)
                continue;
            
            return attribute;
        }
        
        return null;
    }
    
    public static AttributeData? GetAttributeDataByFullTypeName(
        this ImmutableArray<AttributeData> attributes,
        string fullTypeName)
    {
        var globalName = $"global::{fullTypeName}";
        foreach (var attribute in attributes)
        {
            if (attribute.AttributeClass is not { } attributeClass)
                continue;
            
            var attributeTypeName = attributeClass.ToDisplayString(
                SymbolDisplayFormat.FullyQualifiedFormat.WithGenericsOptions(
                    SymbolDisplayGenericsOptions.None));
            
            if (attributeTypeName != globalName)
                continue;
            
            return attribute;
        }
        
        return null;
    }
    
    public static AttributeData? GetAttributeDataByFullTypeName(this Compilation compilation, string fullTypeName)
    {
        return compilation.Assembly
            .GetAttributes()
            .FirstOrDefault(attributeData =>
                attributeData.AttributeClass is not null &&
                attributeData.AttributeClass.ToFullyQualifiedString() ==
                fullTypeName);
    }
    
    public static bool? GetBoolParameterValue(this AttributeData attributeData, string parameterName) =>
        attributeData.GetParameterValue<bool>(parameterName, value => bool.Parse(value.ToString()));
    
    public static string? GetStringParameterValue(this AttributeData attributeData, string parameterName) =>
        attributeData.GetParameterValue<string>(parameterName, value => value.ToString());
    
    private static T? GetParameterValue<T>(
        this AttributeData attributeData,
        string parameterName,
        Func<object, T?> factory)
        where T : struct
    {
        return attributeData.GetParameterValue(parameterName) is { } value ? factory(value) : null;
    }
    
    private static T? GetParameterValue<T>(
        this AttributeData attributeData,
        string parameterName,
        Func<object, T?> factory)
        where T : class
    {
        return attributeData.GetParameterValue(parameterName) is { } value ? factory(value) : null;
    }
    
    private static object? GetParameterValue(
        this AttributeData attributeData,
        string parameterName)
    {
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var argument in attributeData.NamedArguments)
        {
            if (!string.Equals(parameterName, argument.Key, StringComparison.OrdinalIgnoreCase))
                continue;
            
            if (argument.Value.Value is  { } value)
                return value;
        }
        
        return null;
    }
}