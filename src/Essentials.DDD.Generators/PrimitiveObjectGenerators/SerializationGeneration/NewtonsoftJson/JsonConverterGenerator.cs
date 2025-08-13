using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Essentials.DDD.Generators.Extensions;
using Essentials.DDD.Generators.Diagnostics;
using static Essentials.DDD.Generators.PrimitiveObjectGenerators.Dictionaries.KnownAttributes;

namespace Essentials.DDD.Generators.PrimitiveObjectGenerators.SerializationGeneration.NewtonsoftJson;

/// <summary>
/// Генератор классов для сериализации примитивных объектов
/// </summary>
[Generator]
public class JsonConverterGenerator : IIncrementalGenerator
{
    private static readonly Predicate<SyntaxNode> _nodePredicate = node =>
        node is AttributeSyntax { Name: GenericNameSyntax { Identifier.Text: "NewtonsoftJsonSerializable" } };
    
    /// <inheritdoc cref="IIncrementalGenerator.Initialize" />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var valuesProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (syntaxNode, _) => _nodePredicate(syntaxNode),
                transform: (syntaxContext, _) =>
                {
                    if (syntaxContext.Node is not AttributeSyntax { Name: GenericNameSyntax genericNameSyntax })
                        return null;
                    
                    var typeArgument = genericNameSyntax.TypeArgumentList.Arguments.FirstOrDefault();
                    if (typeArgument is null)
                        return null;
                    
                    var compilation = syntaxContext.SemanticModel.Compilation;
                    var model = compilation.GetSemanticModel(typeArgument.SyntaxTree);
                    if (model.GetSymbolInfo(typeArgument).Symbol is not INamedTypeSymbol typeSymbol)
                        return null;
                    
                    if (GetPrimitiveObjectType(typeSymbol) is not { } primitiveObjectType)
                        return null;
                    
                    var attributeTypeName = compilation.GetRequiredGenericTypeName(
                        $"{NEWTONSOFT_JSON_SERIALIZABLE_ATTRIBUTE}`1",
                        typeSymbol);
                    
                    var attributeData = compilation.GetAttributeDataByFullTypeName(attributeTypeName);
                    if (attributeData is null)
                        return null;
                    
                    var @namespace = GetNamespace(attributeData, typeSymbol);
                    return new SerializableModel(
                        compilation,
                        @namespace,
                        typeSymbol,
                        primitiveObjectType);
                })
            .Where(model => model is not null)!
            .WithComparer(SerializableModelComparer.Instance)
            .Collect()
            .SelectMany((array, _) => array.Distinct(SerializableModelComparer.Instance));
        
        context.RegisterSourceOutput(valuesProvider, static (context, model) =>
        {
            var message = DiagnosticsHelper.CreateInfo(
                title: "Формирование модели",
                content: $"Происходит формирование модели числа для типа '{model}'");
            
            context.ReportDiagnostic(Diagnostic.Create(message, Location.None));
            NewtonsoftJsonConverterGenerator.Generate(context, model);
        });
    }
    
    private static string GetNamespace(AttributeData attributeData, INamedTypeSymbol typeSymbol)
    {
        var @namespace = attributeData.GetStringParameterValue("Namespace");
        return string.IsNullOrWhiteSpace(@namespace)
            ? $"{typeSymbol.ContainingNamespace.ToDisplayString()}.NewtonsoftJsonSerialization"
            : @namespace!;
    }
    
    private static INamedTypeSymbol? GetPrimitiveObjectType(INamedTypeSymbol typeSymbol)
    {
        var attributes = typeSymbol.GetAttributes();
        var primitiveObjectTypeAttribute = attributes.GetAttributeDataByFullTypeName(PRIMITIVE_OBJECT_ATTRIBUTE);
        if (primitiveObjectTypeAttribute?.AttributeClass is not { } primitiveObjectType)
            return null;
        
        if (primitiveObjectType.TypeArguments.Length != 1)
            return null;
        
        return primitiveObjectType.TypeArguments[0] as INamedTypeSymbol;
    }
}