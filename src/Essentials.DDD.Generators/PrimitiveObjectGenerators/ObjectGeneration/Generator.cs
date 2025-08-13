using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Essentials.DDD.Generators.Extensions;
using Essentials.DDD.Generators.Diagnostics;
using Essentials.DDD.Generators.PrimitiveObjectGenerators.ObjectGeneration.Services;
using Essentials.DDD.Generators.PrimitiveObjectGenerators.ObjectGeneration.CodeGeneration;
using static Essentials.DDD.Generators.PrimitiveObjectGenerators.Dictionaries.KnownAttributes;

namespace Essentials.DDD.Generators.PrimitiveObjectGenerators.ObjectGeneration;

/// <summary>
/// Генератор моделей примитивных объектов
/// </summary>
[Generator]
public sealed class Generator : IIncrementalGenerator
{
    private static readonly Predicate<SyntaxNode> _nodePredicate = node =>
        node is StructDeclarationSyntax { AttributeLists.Count: > 0 };
    
    /// <inheritdoc cref="IIncrementalGenerator.Initialize" />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var valuesProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (syntaxNode, _) => _nodePredicate(syntaxNode),
                transform: (syntaxContext, _) => CreateModel(syntaxContext))
            .Where(model => model is not null)!
            .WithComparer(PrimitiveObjectModelComparer.Instance)
            .Collect()
            .SelectMany((array, _) => array.Distinct(PrimitiveObjectModelComparer.Instance));
        
        context.RegisterSourceOutput(valuesProvider, static (context, model) =>
        {
            var message = DiagnosticsHelper.CreateInfo(
                title: "Формирование модели",
                content: $"Происходит формирование модели числа для типа '{model.StructTypeShortName}'");
            
            context.ReportDiagnostic(Diagnostic.Create(message, Location.None));
            PrimitiveObjectGenerator.Generate(context, model);
        });
    }
    
    private static PrimitiveObjectModel? CreateModel(GeneratorSyntaxContext syntaxContext)
    {
        var syntax = (StructDeclarationSyntax) syntaxContext.Node;
        var attributeSyntax = syntax.AttributeLists.GetAttributeSyntaxByFullTypeName(
            syntaxContext.SemanticModel,
            PRIMITIVE_OBJECT_ATTRIBUTE);
        
        if (attributeSyntax?.Name is not GenericNameSyntax genericNameSyntax)
            return null;
        
        if (GetPrimitiveObjectType(syntaxContext, genericNameSyntax) is not { } primitiveObjectType)
            return null;
        
        PrimitiveObjectTypeValidator.Validate(primitiveObjectType);
        
        if (GetStructSymbol(syntaxContext) is not { } structSymbol)
            return null;
        
        var attributes = structSymbol.GetAttributes();
        var attribute = attributes.GetAttributeDataByFullTypeName(PRIMITIVE_OBJECT_ATTRIBUTE);
        if (attribute is null)
            return null;
        
        var @namespace = syntax.GetNamespace();
        var generateImplicitOperators = attribute.GetBoolParameterValue("GenerateImplicitOperators") ?? false;
        var needTransform = attribute.GetBoolParameterValue("NeedTransform") ?? false;
        var needExplicitEquals = attribute.GetBoolParameterValue("ExplicitEquals") ?? false;
        var needExplicitCompareTo = attribute.GetBoolParameterValue("ExplicitCompareTo") ?? false;
        
        return new PrimitiveObjectModel(
            syntaxContext.SemanticModel.Compilation,
            @namespace,
            structSymbol,
            primitiveObjectType,
            generateImplicitOperators,
            needTransform,
            needExplicitEquals,
            needExplicitCompareTo);
    }
    
    private static INamedTypeSymbol? GetPrimitiveObjectType(
        GeneratorSyntaxContext syntaxContext,
        GenericNameSyntax genericNameSyntax)
    {
        var primitiveObjectTypeArgument = genericNameSyntax.TypeArgumentList.Arguments.FirstOrDefault();
        if (primitiveObjectTypeArgument is null)
            return null;
        
        var primitiveObjectTypeInfo = syntaxContext.SemanticModel.GetSymbolInfo(primitiveObjectTypeArgument);
        return primitiveObjectTypeInfo.Symbol as INamedTypeSymbol;
    }
    
    private static INamedTypeSymbol? GetStructSymbol(GeneratorSyntaxContext syntaxContext)
    {
        var structSymbolInfo = syntaxContext.SemanticModel.GetDeclaredSymbol(syntaxContext.Node);
        if (structSymbolInfo is not INamedTypeSymbol structSymbol)
            return null;
        
        return structSymbol.TypeArguments.Length > 0 ? null : structSymbol;
    }
}