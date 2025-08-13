using Essentials.DDD.Generators.Extensions;
using static Essentials.DDD.Generators.Services.AttributesBuilder;
using static Essentials.DDD.Generators.Dictionaries.KnownNamespaces;

namespace Essentials.DDD.Generators.PrimitiveObjectGenerators.ObjectGeneration.CodeGeneration;

// ReSharper disable once InconsistentNaming
internal class PrimitiveObjectGenerator_Conversions
{
    public static string Generate(PrimitiveObjectModel model)
    {
        var structTypeName = model.StructTypeGlobalName;
        var primitiveObjectTypeName = model.Compilation.GetRequiredTypeName(model.PrimitiveObjectTypeName);
        var methodImplAttribute = BuildMethodImplAttribute(model.Compilation);
        var excludeFromCodeCoverageAttribute = BuildExcludeFromCodeCoverageAttribute(model.Compilation);
        
        return $$"""
                 using {{COMPILER_SERVICES}};
                 
                 namespace {{model.Namespace}};
                 
                 {{model.Accessibility}} partial struct {{model.StructTypeShortName}}
                 {
                     {{methodImplAttribute}}
                     {{excludeFromCodeCoverageAttribute}}
                     public static implicit operator {{structTypeName}}({{primitiveObjectTypeName}} value) => new(value);
                     
                     {{methodImplAttribute}}
                     {{excludeFromCodeCoverageAttribute}}
                     public static implicit operator {{primitiveObjectTypeName}}({{structTypeName}} model) => model._value;
                 }
                 """;
    }
}